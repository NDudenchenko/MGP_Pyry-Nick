using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;


public class InitializeMultiplayerServices : MonoBehaviour
{

#if !UNITY_SERVER
    async void Start()
    {

        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
#endif
}
