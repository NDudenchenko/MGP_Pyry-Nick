using Unity.Netcode;
using UnityEngine;

public class NetworkHealth : NetworkBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject[] respawnPoints;

    [SerializeField] private GameObject specMode;
    public NetworkVariable<int> _health = new NetworkVariable<int>
        (100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _health.Value = maxHealth;
        
        respawnPoints = GameObject.FindGameObjectsWithTag("RespawnPoint");
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {
            _health.Value -= damage;

            Debug.Log("Took Damage" + damage + "currentHealth is " + _health.Value);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void ReSpawnServerRpc()
    {
        // if (respawnPoints.Length > 0)
        // {
        //     int randSpawnIndex = Random.Range(0, respawnPoints.Length);
        //     this.transform.position = respawnPoints[randSpawnIndex].transform.position;
        //     _health.Value = maxHealth;
        // }
        
        _health.Value = maxHealth;
    }

    private void Update()
    {
        if (_health.Value <= 0)
        {
            // Instantiate(specMode, transform.position, Quaternion.identity);
            // this.NetworkObject.Despawn();

            OnClientRespawn();
        }
    }

    private void OnClientRespawn()
    {
        if (respawnPoints.Length > 0)
        {
            int randSpawnIndex = Random.Range(0, respawnPoints.Length);
            this.transform.position = respawnPoints[randSpawnIndex].transform.position;
            ReSpawnServerRpc();
        }
    }
}
