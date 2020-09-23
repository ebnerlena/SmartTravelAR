public class PingMessageable : INetworkMessageable
{
    public const string MSGTYPE = "PING";
    public PingMessageable()
    {
    }

    public NetworkMessage ToNetworkMessage()
    {
        return new NetworkMessage(MSGTYPE, "PONG");
    }
}