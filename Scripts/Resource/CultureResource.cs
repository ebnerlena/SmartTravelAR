public class CultureResource : IncrementingResource
{
    public CultureResource (float startValue)
    {
        this.Name = "culture";
        this.Value = startValue;
    }

    public override string GetUnitString()
    {
        return "Punkte";
    }
}
