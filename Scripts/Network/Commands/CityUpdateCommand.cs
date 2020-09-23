public class CityUpdateCommand : BasicNetworkCommand<CityUpdateMessageable>
{
    public CityUpdateCommand(CityUpdateMessageable messageable) : base(messageable) { }

    public override void Execute()
    {
        Map.Instance.MovePlayer(messageable.playerId, messageable.cityName);
    }

    public static CityUpdateCommand FromNetworkMessage(NetworkMessage message)
    {
        return new CityUpdateCommand(CityUpdateMessageable.FromNetworkMessage(message));
    }
}