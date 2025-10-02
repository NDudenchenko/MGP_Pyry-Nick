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
    
    private NetworkObject playerNetworkObject;
    private int playersCounter = 0;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    
    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerDisconnected;
        
    }
    
    public override void OnNetworkDespawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerConnected;
    }

    private void OnPlayerConnected(ulong clientId)
    {
        playersCounter++;

        if (clientId == 0)
        {
            Debug.Log("Server connected");
            GameObject player = Instantiate(playerPrefab);
            playerNetworkObject = player.GetComponent<NetworkObject>();

            if (serverSpawnTransform)
            {
                player.transform.position = serverSpawnTransform.position;
                player.transform.rotation = serverSpawnTransform.rotation;
            }

            playerNetworkObject.SpawnWithOwnership(clientId);
        }
        else //Clients
        {
            Debug.Log("Client connected");
            GameObject player = Instantiate(playerPrefab);
            playerNetworkObject = player.GetComponent<NetworkObject>();
            
            if (clientSpawnTransform)
            {
                player.transform.position = clientSpawnTransform.position;
                player.transform.rotation = clientSpawnTransform.rotation;
            }
            
            playerNetworkObject.SpawnWithOwnership(clientId);
        }
    }
    
    private void OnPlayerDisconnected(ulong clientId)
    {
        playersCounter--;
        
        if (HasAuthority && playerNetworkObject && playerNetworkObject.IsSpawned)
        {
            playerNetworkObject.Despawn();
        }
    }


}
