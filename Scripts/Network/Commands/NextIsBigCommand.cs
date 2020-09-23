public class NextIsBigCommand : BasicNetworkCommand<NextIsBigMessageable>
{
    public NextIsBigCommand(NextIsBigMessageable messageable) : base(messageable) { }

    public override void Execute()
    {
        GameManager.Instance.NetworkPlayer?.SetNextBufferSize(messageable.Size);
    }

    public static NextIsBigCommand FromNetworkMessage(NetworkMessage message)
    {
        return new NextIsBigCommand(NextIsBigMessageable.FromNetworkMessage(message));
    }
}