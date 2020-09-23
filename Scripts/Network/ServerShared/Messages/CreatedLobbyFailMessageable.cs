public class CreatedLobbyFailMessageable : LobbyRelatedMessageable
{
    public const string MSGTYPE = "CRLBY_FAIL";
    public CreatedLobbyFailMessageable(string lobbyId) : base (lobbyId) { }

    protected override string GetMSGTYPE() { return MSGTYPE; }
}