public class JoinLobbyCommand : INetworkCommand
{
    public const string MSGTYPE = "JOLBY";

    private string lobbyId;
    public JoinLobbyCommand(string lobbyId)
    {
        this.lobbyId = lobbyId;
    }

    public void Execute()
    {
        GameManager.Instance.NetworkPlayer?.ActuallyJoinLobby(lobbyId);
    }

    public NetworkMessage ToNetworkMessage()
    {
        return new NetworkMessage(MSGTYPE, lobbyId);
    }

    public static JoinLobbyCommand FromNetworkMessage(NetworkMessage message)
    {
        return new JoinLobbyCommand(message.body);
    }
}