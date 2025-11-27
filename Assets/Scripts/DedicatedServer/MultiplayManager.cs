
using UnityEngine;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using Unity.Services.Multiplayer;


public class MultiplayManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        if (Application.platform == RuntimePlatform.LinuxServer)
        {
            Application.targetFrameRate = 60;
            
            await UnityServices.InitializeAsync();
            
            //ServerConfig serverConfig = MultiplayerService.Instance;
            //MultiplayerService.Instance.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
