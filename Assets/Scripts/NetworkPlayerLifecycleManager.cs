using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Multiplayer;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerLifecycleManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Transform serverSpawnTransform;
    [SerializeField]
    private Transform clientSpawnTransform;

    [SerializeField]
    private List<Transform> spawnPoints;
    
    private NetworkObject _playerNetworkObject;

    private string _sessionId = String.Empty;
    private string _sessionCode = String.Empty;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerDisconnected;
        }
    }
    
    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnPlayerDisconnected;
        }
    }

    private void OnPlayerConnected(ulong clientId)
    {
        GameObject player = Instantiate(playerPrefab, spawnPoints[(int)clientId].position, spawnPoints[(int)clientId].rotation);
        player.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);

        if (clientId == 0)
        {
            StartSessionAsHost();
        }
        else
        {
            JoinSessionAsClient();
        }
    }

    private void OnPlayerDisconnected(ulong clientId)
    {
        if (HasAuthority && _playerNetworkObject && _playerNetworkObject.IsSpawned)
        {
            _playerNetworkObject.Despawn();
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        GameObject player = Instantiate(playerPrefab, spawnPoints[(int)clientId].position, spawnPoints[(int)clientId].rotation);
        player.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    }
    
    async Task StartSessionAsHost()
    {
        var options = new SessionOptions
        {
            MaxPlayers = 2
        }.WithRelayNetwork(); // or WithDistributedAuthorityNetwork() to use Distributed Authority instead of Relay
        var session = await MultiplayerService.Instance.CreateSessionAsync(options);
        Debug.Log($"Session {session.Id} created! Join code: {session.Code}");
        
        _sessionId = session.Id;
        _sessionCode = session.Code;
    }

    async Task JoinSessionAsClient()
    {
        if (_sessionId.Length != 0)
        {
            var session = await MultiplayerService.Instance.JoinSessionByIdAsync(_sessionId);
        }
    }
}
