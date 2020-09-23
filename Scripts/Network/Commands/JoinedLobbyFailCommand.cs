public class JoinedLobbyFailCommand : BasicNetworkCommand<LobbyRelatedMessageable>
{
    public JoinedLobbyFailCommand(LobbyRelatedMessageable messageable) : base(messageable) { }

    public override void Execute()
    {
        UnityEngine.Debug.Log("Lobby \"" + messageable.lobbyId + "\" does not exist.");
        GameManager.Instance.SetErrorMessage(ErrorMessageType.JoinLobbyIDError, "Lobby \"" + messageable.lobbyId + "\" does not exist.");
    }

    public static JoinedLobbyFailCommand FromNetworkMessage(NetworkMessage message)
    {
        return new JoinedLobbyFailCommand(LobbyRelatedMessageable.FromNetworkMessage(message));
    }
}