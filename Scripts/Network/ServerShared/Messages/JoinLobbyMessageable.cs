using Newtonsoft.Json;

public class JoinLobbyMessageable : LobbyRelatedMessageable
{
    public const string MSGTYPE = "JOLBY";
    public PlayerInfo playerInfo { get; }

    public JoinLobbyMessageable(string lobbyId, PlayerInfo playerInfo) : base(lobbyId) 
    {
        this.playerInfo = playerInfo;
    }
    protected override string GetMSGTYPE() { return MSGTYPE; }

    public override NetworkMessage ToNetworkMessage()
    {
        string jsonString = JsonConvert.SerializeObject(this);
        return new NetworkMessage(GetMSGTYPE(), jsonString);
    }

    public static new JoinLobbyMessageable FromNetworkMessage(NetworkMessage message)
    {
        return JsonConvert.DeserializeObject<JoinLobbyMessageable>(message.body);
    }

}