using Newtonsoft.Json;

public class WeightMessageable : INetworkMessageable
{
    public string lobbyId { get; }
    public float daysLeftWeight { get; }
    public float culturePointsWeight { get; }
    public string[] resourceNames { get; }
    public float[] weights { get; }

    protected virtual string GetMSGTYPE() { return "WEGHT"; }

    public WeightMessageable(string lobbyId, float daysLeftWeight, float culturePointsWeight, string[] resourceNames, float[] weights)
    {
        this.lobbyId = lobbyId;
        this.daysLeftWeight = daysLeftWeight;
        this.culturePointsWeight = culturePointsWeight;
        this.resourceNames = resourceNames;
        this.weights = weights;
    }

    public virtual NetworkMessage ToNetworkMessage()
    {
        string jsonString = JsonConvert.SerializeObject(this);
        return new NetworkMessage(GetMSGTYPE(), jsonString);
    }

    public static WeightMessageable FromNetworkMessage(NetworkMessage message)
    {
        return JsonConvert.DeserializeObject<WeightMessageable>(message.body);
    }
}