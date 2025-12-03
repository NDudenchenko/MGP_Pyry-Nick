using System.Threading;
using Unity.Netcode;
using Unity.Services.Multiplayer;

public class MatchMakeButton : NetworkBehaviour

{

    public async void PressMatchMake()
    {
        var matchmakerOptions = new MatchmakerOptions
        {
            QueueName = "Friendly"
        };

        var sessionOptions = new SessionOptions()
        {
            MaxPlayers = 2
        }.WithDirectNetwork();

        var matchmakerCancellationSource = new CancellationTokenSource();

        ISession session = await MultiplayerService.Instance.MatchmakeSessionAsync(matchmakerOptions, sessionOptions, matchmakerCancellationSource.Token);
    }

}
