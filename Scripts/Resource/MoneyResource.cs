public class MoneyResource : DecrementingResource
{
    public MoneyResource(float startValue)
    {
        this.Name = "money";
        this.Value = startValue;
    }
    public override string GetUnitString()
    {
        return "€";
    }
}
