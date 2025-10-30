using Unity.Netcode;
using UnityEngine;

public class NetworkHealth : NetworkBehaviour
{
    [SerializeField]private int currentHealth;
    [SerializeField] private int maxHealth;


    public NetworkVariable<int> _health = new NetworkVariable<int>
        (100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _health.Value = maxHealth;
    }

    [ServerRpc(RequireOwnership = false)]

    public void TakeDamageServerRpc(int damage)
    {
            _health.Value -= damage;

            Debug.Log("Took Damage" + damage + "currentHealth is " + _health.Value);
    }

    private void Update()
    {
        if(_health.Value <= 0)
        {
            this.NetworkObject.Despawn();
        }
    }

    
}
