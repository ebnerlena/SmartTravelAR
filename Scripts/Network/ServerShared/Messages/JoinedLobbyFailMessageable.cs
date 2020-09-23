public class JoinedLobbyFailMessageable : LobbyRelatedMessageable
{
    public const string MSGTYPE = "JOLBY_FAIL";
    public JoinedLobbyFailMessageable(string lobbyId) : base (lobbyId) { }

    protected override string GetMSGTYPE() { return MSGTYPE; }
}