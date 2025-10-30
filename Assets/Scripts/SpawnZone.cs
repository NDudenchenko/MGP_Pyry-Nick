using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnZone : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] ServerSpawnPosition;
    [SerializeField]
    private GameObject[] ClientSpawnPosition;

    private GameObject playerPrefab;

    private void Awake()
    {
        ServerSpawnPosition = GameObject.FindGameObjectsWithTag("ServerSpawnPoint");
        ClientSpawnPosition = GameObject.FindGameObjectsWithTag("ClientSpawnPoint");
    }

    void Start()
    {
        //playerPrefab = transform.parent.gameObject;

    }

    void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            transform.position = ServerSpawnPosition[0].transform.position;
        }
        else if (IsClient)
        {
            TeleportPlayer_ServerRpc();
        }
        else
        {
            return;
        }
    }

    [ServerRpc]
    public void TeleportPlayer_ServerRpc()
    {
        TeleportPlayer_ClientRpc();
    }
    
    [ClientRpc]
    public void TeleportPlayer_ClientRpc()
    {
        transform.position = ClientSpawnPosition[0].transform.position;
    }
}
