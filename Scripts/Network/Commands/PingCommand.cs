public class PingCommand : BasicNetworkCommand<PingMessageable>
{
    public PingCommand() : base(new PingMessageable()) { }

    public override void Execute()
    {
        UnityEngine.Debug.Log("PING :)");

    }
    // keeping signature to match delegate in MessageHandler
    public static PingCommand FromNetworkMessage(NetworkMessage message)
    {
        return new PingCommand();
    }
}