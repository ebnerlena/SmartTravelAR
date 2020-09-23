using Newtonsoft.Json;

public class ScoreUpdateMessageable : INetworkMessageable
{
    public const string MSGTYPE = "SCO_UPD";
    public string playerId { get; }
    public float score { get; }
    public float culturePoints { get; }
    public float daysLeft { get; }
    public string[] resourceNames { get; }
    public float[] rawValues { get; }

    public ScoreUpdateMessageable(string playerId, float score, float culturePoints, float daysLeft, string[] resourceNames, float[] rawValues)
    {
        this.playerId = playerId;
        this.score = score;
        this.culturePoints = culturePoints;
        this.daysLeft = daysLeft;
        this.resourceNames = resourceNames;
        this.rawValues = rawValues;
    }

    public static ScoreUpdateMessageable FromScoreReq(ScoreUpdateRequestMessageable req, float score)
    {
        return new ScoreUpdateMessageable(req.playerId, score, req.culturePoints, req.daysLeft, req.resourceNames, req.rawValues);
    }

    public NetworkMessage ToNetworkMessage()
    {
        string jsonString = JsonConvert.SerializeObject(this);
        return new NetworkMessage(MSGTYPE, jsonString);
    }

    public static ScoreUpdateMessageable FromNetworkMessage(NetworkMessage message)
    {
        return JsonConvert.DeserializeObject<ScoreUpdateMessageable>(message.body);
    }
}