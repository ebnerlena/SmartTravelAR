public class NextIsBigMessageable : INetworkMessageable {
    public const string MSGTYPE = "NXT_BIG";
    public int Size { get; }

    public NextIsBigMessageable(int size) {
        this.Size = size;
    }

    public NetworkMessage ToNetworkMessage()
    {
        return new NetworkMessage(MSGTYPE, Size.ToString());
    }

    public static NextIsBigMessageable FromNetworkMessage(NetworkMessage message)
    {
        bool succ = int.TryParse(message.body, out int size);
        return new NextIsBigMessageable(succ ? size : -1);
    }
}