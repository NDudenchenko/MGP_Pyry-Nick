using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrap : MonoBehaviour
{

    private void Start()
    {
#if UNITY_SERVER
        SceneManager.LoadScene("Server", LoadSceneMode.Additive);
#else
        SceneManager.LoadScene("Client", LoadSceneMode.Additive);
#endif
    }


}
