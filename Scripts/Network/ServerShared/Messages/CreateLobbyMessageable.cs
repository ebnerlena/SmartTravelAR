public class CreateLobbyMessageable : WeightMessageable
{
    public const string MSGTYPE = "CRLBY";
    protected override string GetMSGTYPE() { return MSGTYPE; }
    public CreateLobbyMessageable(string lobbyId, float daysLeftWeight, float culturePointsWeight, string[] resourceNames, float[] weights) 
        : base (lobbyId, daysLeftWeight, culturePointsWeight, resourceNames, weights) { }

    public static CreateLobbyMessageable FromOtherWeightMessageable(WeightMessageable wm)
    {
        return new CreateLobbyMessageable(wm.lobbyId, wm.daysLeftWeight, wm.culturePointsWeight, wm.resourceNames, wm.weights);
    }
}