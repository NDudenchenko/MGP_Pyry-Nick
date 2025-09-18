using Unity.Netcode;
using UnityEngine;

public class NetworkHealth : NetworkBehaviour
{
    [SerializeField]private int currentHealth;
    [SerializeField] private int maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    [ServerRpc(RequireOwnership = false)]

    public void TakeDamageServerRpc(int damage)
    {
        

        
            currentHealth -= damage;

            Debug.Log("Took Damage" + damage + "currentHealth is " + currentHealth);
        

        
    }
    
}
