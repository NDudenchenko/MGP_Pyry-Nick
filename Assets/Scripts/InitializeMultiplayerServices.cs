using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeMultiplayerServices : MonoBehaviour
{
    async void Start()
    {

        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
            SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
