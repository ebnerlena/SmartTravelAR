public class StartGameCommand : BasicNetworkCommand<StartGameMessageable>
{
    public StartGameCommand(StartGameMessageable messageable) : base(messageable) { }

    public override void Execute()
    {
        GameManager.Instance.StartGame();
    }

    public static StartGameCommand FromNetworkMessage(NetworkMessage message)
    {
        return new StartGameCommand(StartGameMessageable.FromNetworkMessage(message));
    }
}
