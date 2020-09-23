public class ScoreUpdateCommand : BasicNetworkCommand<ScoreUpdateMessageable>
{
    public ScoreUpdateCommand(ScoreUpdateMessageable messageable) : base(messageable) { }

    public override void Execute()
    {
        if(messageable.score >= 0)
            GameManager.Instance.PlayerListManager.UpdatePlayer(messageable);
    }

    public static ScoreUpdateCommand FromNetworkMessage(NetworkMessage message)
    {
        return new ScoreUpdateCommand(ScoreUpdateMessageable.FromNetworkMessage(message));
    }
}