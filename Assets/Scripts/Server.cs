#if UNITY_SERVER || ENABLE_UCS_SERVER
using System.Threading.Tasks;
using Unity.Services.Authentication.Server;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using System;
using Unity.Netcode;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Services
{
    public class Server : MonoBehaviour
    {
#if UNITY_SERVER || ENABLE_UCS_SERVER

        // Server Query Protocol
        private const ushort k_DefaultMaxPlayers = 2;
        private const string k_DefaultServerName = "DefaultServerName";
        private const string k_DefaultGameType = "DefaultGameType";
        private const string k_DefaultBuildId = "1310656";
        private const string k_DefaultMap = "DefaultMap";

        [SerializeField] private NetworkObject _playerNetworkObject;
        private Transform[] _spawnLocations;

        IMultiplaySessionManager m_SessionManager;

        private async void Start()
        {
            try
            {
                await UnityServices.InitializeAsync();
                await ConnectToMultiplay();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private async Task ConnectToMultiplay()
        {
            if (UnityServices.Instance.GetMultiplayerService() != null)
            {
                // Authenticate
                await ServerAuthenticationService.Instance.SignInFromServerAsync();
                var token = ServerAuthenticationService.Instance.AccessToken;

                // Callbacks should be used to ensure proper state of the server allocation.
                // Awaiting the StartMultiplaySessionManagerAsync won't guarantee proper state.
                var callbacks = new MultiplaySessionManagerEventCallbacks();
                callbacks.Allocated += OnServerAllocatedCallback;

                var sessionManagerOptions = new MultiplaySessionManagerOptions()
                {
                    SessionOptions = new SessionOptions()
                    {
                        MaxPlayers = k_DefaultMaxPlayers
                    }.WithDirectNetwork(),

                    // Server options are REQUIRED for the underlying SQP server
                    MultiplayServerOptions = new MultiplayServerOptions(
                        serverName: k_DefaultServerName,
                        gameType: k_DefaultGameType,
                        buildId: k_DefaultBuildId,
                        map: k_DefaultMap,
                        autoReady: false
                    ),
                    Callbacks = callbacks
                };
                m_SessionManager = await MultiplayerServerService.Instance.StartMultiplaySessionManagerAsync(sessionManagerOptions);

               // Ensure that the session is only accessed after the allocation happened.
               // Otherwise you risk the Session being in an uninitialized state.
                async void OnServerAllocatedCallback(IMultiplayAllocation obj)
                {
                    Debug.Log("[Multiplay] Server is allocated");
                    var session = m_SessionManager.Session;
                    
                    NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                    NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
                    NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApproval;
                    
                    NetworkManager.Singleton.StartServer();
                    NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;

                    NetworkManager.Singleton.SceneManager.LoadScene("PyryScene", UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    


                    await m_SessionManager.SetPlayerReadinessAsync(true);
                    Debug.Log("[Multiplay] Server is ready to accept players");
                }
            }
        }

        private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            if(sceneName == "PyryScene")
            {
                InitSpawnLocations();
                SpawnPlayerObjects();
            }

        }

        private void InitSpawnLocations()
        {
            // Gather all Spawn Location objects
            var spawnLocationObjects = GameObject.FindGameObjectsWithTag("SpawnLocation");

            // If none are present, add the World Origin as the only spawn location
            if (spawnLocationObjects.Length == 0)
            {
                var go = Instantiate(new GameObject("SpawnLocation"), Vector3.zero, Quaternion.identity);
                _spawnLocations = new[] { go.transform };
                return;
            }

            // Convert existing Spawn Location objects into an array of Transforms
            _spawnLocations = new Transform[spawnLocationObjects.Length];
            for (int i = 0; i < spawnLocationObjects.Length; i++)
                _spawnLocations[i] = spawnLocationObjects[i].transform;
        }

        private void SpawnPlayerObjects()
        {
            // Check Player Prefab
            if (_playerNetworkObject == null)
            {
                Debug.LogError("Player Prefab is not assigned!");
                return;
            }

            var playerObjectIndex = 0;
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                // Cycle through spawn locations
                var spawnLocationIndex = playerObjectIndex % _spawnLocations.Length;
                var spawnPos = _spawnLocations[spawnLocationIndex].position;

                var playerObj = Instantiate(_playerNetworkObject, spawnPos, Quaternion.identity);
                playerObj.SpawnAsPlayerObject(client.ClientId, true);

                playerObjectIndex++;
            }
        }

        public void OnDestroy()
        {
            if (NetworkManager.Singleton == null) return;
            
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
            
            NetworkManager.Singleton.ConnectionApprovalCallback -= ConnectionApproval;
        }
        
        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"Client {clientId} connected");
        }

        private void OnClientDisconnected(ulong clientId)
        {
            Debug.Log($"Client {clientId} disconnected");
        }
        
        private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            if (NetworkManager.Singleton.ConnectedClients.Count >= 2)
            {
                // Reject (Game full)
                response.Approved = false;
                response.CreatePlayerObject = false;
                response.Reason = "Game full";
                Debug.Log($"Rejected client {request.ClientNetworkId}: Game full ({NetworkManager.Singleton.ConnectedClients.Count}/{2})");
            }
            else
            {
                // Approve
                response.Approved = true;
                response.CreatePlayerObject = true;
            
                Debug.Log($"Approved client {request.ClientNetworkId}: {NetworkManager.Singleton.ConnectedClients.Count + 1}/{2} players");
            }
            
            // Instant response
            response.Pending = false;
        }
#endif
    }
}