using System;

public class TripUpdateCommand : BasicNetworkCommand<TripUpdateMessegable>
{
    public TripUpdateCommand(TripUpdateMessegable messageable) : base(messageable) { }

    public override void Execute()
    {
        GameManager.Instance.ExecuteOnMain(() =>
            Map.Instance.MoveBetweenCities(messageable.playerId, messageable.startCityName, messageable.endCityName, messageable.tripTimeInSeconds, messageable.transportTypeName)
           
        );
        GameManager.Instance.PlayerListManager.UpdateCityOfPlayer(messageable.playerId, messageable.endCityName);
    }

    public static TripUpdateCommand FromNetworkMessage(NetworkMessage message)
    {
        return new TripUpdateCommand(TripUpdateMessegable.FromNetworkMessage(message));
    }
}