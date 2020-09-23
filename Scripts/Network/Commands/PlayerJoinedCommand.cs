public class PlayerJoinedCommand : BasicNetworkCommand<PlayerJoinedMessageable>
{
    public PlayerJoinedCommand(PlayerJoinedMessageable messageable) : base(messageable) { }

    public override void Execute()
    {
        UnityEngine.Debug.Log("executing add "+messageable.playerInfo.name);
        GameManager.Instance.ExecuteOnMain(() => GameManager.Instance.PlayerListManager.AddEnemyPlayers(messageable.playerInfo));
    }

    public static PlayerJoinedCommand FromNetworkMessage(NetworkMessage message)
    {
        return new PlayerJoinedCommand(PlayerJoinedMessageable.FromNetworkMessage(message));
    }
}