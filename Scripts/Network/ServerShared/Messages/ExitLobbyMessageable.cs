public class ExitLobbyMessageable : INetworkMessageable
{
    public const string MSGTYPE = "EXL";

    public NetworkMessage ToNetworkMessage()
    {
        return new NetworkMessage(MSGTYPE, string.Empty);
    }

   /* public static StartGameMessageable FromNetworkMessage(NetworkMessage message)
    {
        return new StartGameMessageable();
    } */
}