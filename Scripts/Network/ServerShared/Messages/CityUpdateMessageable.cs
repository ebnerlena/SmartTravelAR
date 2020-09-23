using Newtonsoft.Json;

public class CityUpdateMessageable : INetworkMessageable
{
    public const string MSGTYPE = "CITY_UPD";
    public string playerId { get; }
    public string cityName { get; }
    //public string stayTime { get; }
    //public string[] usedResourceNames { get; }
    //public float[] usedResourceValues { get; }

    public CityUpdateMessageable(string playerId, string cityName/*, string stayTime, string[] usedResourceNames, float[] usedResourceValues*/)
    {
        this.playerId = playerId;
        this.cityName = cityName;
        //this.stayTime = stayTime;
        //this.usedResourceNames = usedResourceNames;
        //this.usedResourceValues = usedResourceValues;
    }

    public NetworkMessage ToNetworkMessage()
    {
        string jsonString = JsonConvert.SerializeObject(this);
        return new NetworkMessage(MSGTYPE, jsonString);
    }

    public static CityUpdateMessageable FromNetworkMessage(NetworkMessage message)
    {
        return JsonConvert.DeserializeObject<CityUpdateMessageable>(message.body);
    }
}