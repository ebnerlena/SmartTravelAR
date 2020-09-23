public class JoinLobbyAsViewerMessageable : LobbyRelatedMessageable
{
    public const string MSGTYPE = "JOLBY_VIEW";
    public JoinLobbyAsViewerMessageable(string lobbyId) : base(lobbyId) { }
    public JoinLobbyAsViewerMessageable(LobbyRelatedMessageable lobbyMsg) : base (lobbyMsg.lobbyId) { }

    protected override string GetMSGTYPE() { return MSGTYPE; }

    public static new JoinLobbyAsViewerMessageable FromNetworkMessage(NetworkMessage message)
    {
        return new JoinLobbyAsViewerMessageable(LobbyRelatedMessageable.FromNetworkMessage(message));
    }
}