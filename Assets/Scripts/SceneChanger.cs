using Unity.Netcode;
using UnityEngine;

public class SceneChanger : NetworkBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsServer) return;


        if (Input.GetKeyDown(KeyCode.R))
        {
            NetworkManager.Singleton.SceneManager.LoadScene("FightScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
