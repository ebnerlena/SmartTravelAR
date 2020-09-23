public abstract class BasicNetworkCommand<T> : INetworkCommand where T : INetworkMessageable
{
    protected T messageable;
    public BasicNetworkCommand(T messageable)
    {
        this.messageable = messageable;
    }

    public abstract void Execute();

    public NetworkMessage ToNetworkMessage()
    {
        return messageable.ToNetworkMessage();
    }
}