using System.Collections.Generic;
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
}
