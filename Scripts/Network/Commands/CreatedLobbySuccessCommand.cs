using System;
using System.Collections.Generic;

public class CreatedLobbySuccessCommand : BasicNetworkCommand<LobbyRelatedMessageable>
{
    public CreatedLobbySuccessCommand(LobbyRelatedMessageable messageable) : base(messageable) { }
    public override void Execute()
    {
        UnityEngine.Debug.Log("Successfully created Lobby " + messageable.lobbyId);
        GameManager.Instance.CreatedLobbySuccess();
    }

    public static CreatedLobbySuccessCommand FromNetworkMessage(NetworkMessage message)
    {
        return new CreatedLobbySuccessCommand(LobbyRelatedMessageable.FromNetworkMessage(message));
    }
}