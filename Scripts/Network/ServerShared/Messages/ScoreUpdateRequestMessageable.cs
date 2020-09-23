using Newtonsoft.Json;

public class ScoreUpdateRequestMessageable : INetworkMessageable
{
    public const string MSGTYPE = "SCO_REQ";
    public string playerId { get; }
    public float culturePoints { get; }
    public float daysLeft { get; }
    public string[] resourceNames { get; }
    public float[] rawValues { get; }

    public ScoreUpdateRequestMessageable(string playerId, float daysLeft, float culturePoints, string[] resourceNames, float[] rawValues)
    {
        this.playerId = playerId;
        this.daysLeft = daysLeft;
        this.culturePoints = culturePoints;
        this.resourceNames = resourceNames;
        this.rawValues = rawValues;
    }

    public NetworkMessage ToNetworkMessage()
    {
        string jsonString = JsonConvert.SerializeObject(this);
        return new NetworkMessage(MSGTYPE, jsonString);
    }

    public static ScoreUpdateRequestMessageable FromNetworkMessage(NetworkMessage message)
    {
        return JsonConvert.DeserializeObject<ScoreUpdateRequestMessageable>(message.body);
    }
}