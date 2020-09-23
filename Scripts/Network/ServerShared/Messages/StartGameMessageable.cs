public class StartGameMessageable : INetworkMessageable
{
    public const string MSGTYPE = "STGM";
    public StartGameMessageable() { }

    public NetworkMessage ToNetworkMessage()
    {
        return new NetworkMessage(MSGTYPE,string.Empty);
    }

    public static StartGameMessageable FromNetworkMessage(NetworkMessage message)
    {
        return new StartGameMessageable();
    }
}