public class TimeResource : DecrementingResource
{
    public TimeResource(float startValue)
    {
        this.Name = "time";
        this.Value = startValue; //in h
    }
    public override string GetUnitString()
    {
        return "Stunden";
    }
}
