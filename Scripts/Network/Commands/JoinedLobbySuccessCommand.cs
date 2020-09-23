using System;
using System.Collections.Generic;

public class JoinedLobbySuccessCommand : BasicNetworkCommand<JoinedLobbySuccessMessageable>
{
    public JoinedLobbySuccessCommand(JoinedLobbySuccessMessageable messageable) : base(messageable) { }

    public override void Execute()
    {
        GameManager.Instance.SetupPlayerOnLobbyJoin();
        GameManager.Instance.NetworkPlayer?.ActuallyJoinLobby(messageable.lobbyId);
        GameManager.Instance.PlayerListManager.AddEnemyPlayers(messageable.lobbyPlayers);
    }

    public static JoinedLobbySuccessCommand FromNetworkMessage(NetworkMessage message)
    {
        return new JoinedLobbySuccessCommand(JoinedLobbySuccessMessageable.FromNetworkMessage(message));
    }
}