using System;
using Newtonsoft.Json;

public class TripUpdateMessegable : INetworkMessageable
{
    public const string MSGTYPE = "TRIP_UPD";
    public string playerId { get; }
    public float tripTimeInSeconds { get; }
    public DateTime startTime { get; }
    public string startCityName { get; }
    public string endCityName { get; }
    public string transportTypeName { get; }

    public TripUpdateMessegable(string playerId, float tripTimeInSeconds, DateTime startTime, string startCityName, string endCityName, string transportTypeName)
    {
        this.playerId = playerId;
        this.tripTimeInSeconds = tripTimeInSeconds;
        this.startTime = startTime;
        this.startCityName = startCityName;
        this.endCityName = endCityName;
        this.transportTypeName = transportTypeName;
    }

    public NetworkMessage ToNetworkMessage()
    {
        string jsonString = JsonConvert.SerializeObject(this);
        return new NetworkMessage(MSGTYPE, jsonString);
    }

    public static TripUpdateMessegable FromNetworkMessage(NetworkMessage message)
    {
        return JsonConvert.DeserializeObject<TripUpdateMessegable>(message.body);
    }
}