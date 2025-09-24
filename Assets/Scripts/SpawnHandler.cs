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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerPrefab = transform.parent.gameObject;

    }

    // Update is called once per frame
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

        // if (IsServer)
        // {
        //     if (NetworkObject.OwnerClientId == 0)
        //     {
        //         if (ServerSpawnPosition != null)
        //             transform.position = ServerSpawnPosition[0].transform.position;
        //     }
        //     else if (NetworkObject.OwnerClientId != 0)
        //     {
        //         if (ClientSpawnPosition != null)
        //         {
        //             TeleportPlayer_ServerRpc();
        //         }
        //     }
        //         
        // }
        // else if (IsClient)
        // {
        //     if (ClientSpawnPosition != null)
        //     {
        //         TeleportPlayer_ServerRpc();
        //         
        //     }
        //
        // }
    }

    //[ServerRpc]
    [ServerRpc]
    public void TeleportPlayer_ServerRpc()
    {
        //transform.position = ClientSpawnPosition[0].transform.position;
        TeleportPlayer_ClientRpc();
    }
    
    [ClientRpc]
    public void TeleportPlayer_ClientRpc()
    {
        transform.position = ClientSpawnPosition[0].transform.position;
    }
}
