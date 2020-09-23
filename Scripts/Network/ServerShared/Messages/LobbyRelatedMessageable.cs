public class LobbyRelatedMessageable : INetworkMessageable
{
    public string lobbyId { get; }
    public LobbyRelatedMessageable(string lobbyId)
    {
        this.lobbyId = lobbyId;
    }

    protected virtual string GetMSGTYPE() { return "LOBBY"; }

    public virtual NetworkMessage ToNetworkMessage()
    {
        return new NetworkMessage(GetMSGTYPE(), lobbyId);
    }

    public static LobbyRelatedMessageable FromNetworkMessage(NetworkMessage message)
    {
        return new LobbyRelatedMessageable(message.body);
    }
}