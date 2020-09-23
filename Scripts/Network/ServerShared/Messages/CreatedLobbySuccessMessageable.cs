using System;
using System.Collections.Generic;

public class CreatedLobbySuccessMessageable : LobbyRelatedMessageable
{
    public const string MSGTYPE = "CRLBY_SUCC";
    public CreatedLobbySuccessMessageable(string lobbyId) : base(lobbyId) { }
    protected override string GetMSGTYPE() { return MSGTYPE; }
}