public class CO2Resource : IncrementingResource
{
    public CO2Resource(float startValue) 
    {
        this.Name = "co2";
        this.Value = startValue;
    }

    public override string GetUnitString()
    {
        return "kg";
    }
}