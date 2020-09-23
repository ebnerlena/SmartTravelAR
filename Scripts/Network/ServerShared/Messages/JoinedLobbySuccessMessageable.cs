using Newtonsoft.Json;

public class JoinedLobbySuccessMessageable : INetworkMessageable
{
    public const string MSGTYPE = "JOLBY_SUCC";
    public string lobbyId { get; }
    public PlayerInfo[] lobbyPlayers { get; }
    public JoinedLobbySuccessMessageable(string lobbyId, PlayerInfo[] lobbyPlayers)
    {
        this.lobbyId = lobbyId;
        this.lobbyPlayers = lobbyPlayers;
    }

    public NetworkMessage ToNetworkMessage()
    {
        string jsonString = JsonConvert.SerializeObject(this);
        return new NetworkMessage(MSGTYPE, jsonString);
    }

    public static JoinedLobbySuccessMessageable FromNetworkMessage(NetworkMessage message)
    {
        return JsonConvert.DeserializeObject<JoinedLobbySuccessMessageable>(message.body);
    }

    public static readonly JoinedLobbySuccessMessageable JoinTestLobby =
        new JoinedLobbySuccessMessageable("test", new PlayerInfo[0]);
}