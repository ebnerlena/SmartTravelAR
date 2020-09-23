public class CreatedLobbyFailCommand : BasicNetworkCommand<LobbyRelatedMessageable>
{
    public CreatedLobbyFailCommand(LobbyRelatedMessageable messageable) : base(messageable) { }

    public override void Execute()
    {
        UnityEngine.Debug.Log("Lobby Id is already taken.");
        GameManager.Instance.SetErrorMessage(ErrorMessageType.CreateLobbyError,"SpielID existiert bereits.");
    }

    public static CreatedLobbyFailCommand FromNetworkMessage(NetworkMessage message)
    {
        return new CreatedLobbyFailCommand(LobbyRelatedMessageable.FromNetworkMessage(message));
    }
}