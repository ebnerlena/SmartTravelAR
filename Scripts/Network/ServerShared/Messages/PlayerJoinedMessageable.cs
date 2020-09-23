using Newtonsoft.Json;

public class PlayerJoinedMessageable : INetworkMessageable
{
    public const string MSGTYPE = "PLYR_JOIN";
    public PlayerInfo playerInfo { get; }

    public PlayerJoinedMessageable(string id, string name, string avatarType)
    {
        playerInfo = new PlayerInfo(id, name, avatarType);
    }

    public PlayerJoinedMessageable(PlayerInfo playerInfo) 
    {
        this.playerInfo = playerInfo;
    }

    public NetworkMessage ToNetworkMessage()
    {
        string playerInfoString = JsonConvert.SerializeObject(playerInfo);
        return new NetworkMessage(MSGTYPE, playerInfoString);
    }

    public static PlayerJoinedMessageable FromNetworkMessage(NetworkMessage message)
    {
        PlayerInfo info = JsonConvert.DeserializeObject<PlayerInfo>(message.body);
        return new PlayerJoinedMessageable(info);
    }
}