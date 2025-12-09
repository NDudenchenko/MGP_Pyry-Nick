using System.Threading;
using System;
using Unity.Netcode;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;


public class MatchMakeButton : NetworkBehaviour

{
#if !UNITY_SERVER

    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button cancelButton;

    private CancellationTokenSource matchmakerCancellationSource;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            ToggleButtons();
            StartMatchMake();
            
        });

        cancelButton.onClick.AddListener(() =>
        {
            ToggleButtons();
            matchmakerCancellationSource?.Cancel();
        });
    }

    private async void StartMatchMake()
    {
        try
        {
            var matchmakerOptions = new MatchmakerOptions
            {
                QueueName = "FirstQueue"
            };

            var sessionOptions = new SessionOptions()
            {
                MaxPlayers = 2
            }.WithDirectNetwork();

            matchmakerCancellationSource = new CancellationTokenSource();

            var session = await MultiplayerService.Instance.MatchmakeSessionAsync(matchmakerOptions, sessionOptions, matchmakerCancellationSource.Token);
            Debug.Log("Joingin Session..");
            
            //await MultiplayerService.Instance.JoinSessionByCodeAsync(session.Code);
            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void ToggleButtons()
    {
        startButton.gameObject.SetActive(!startButton.gameObject.activeSelf);
        cancelButton.gameObject.SetActive(!cancelButton.gameObject.activeSelf);
    }
    

#endif
}
