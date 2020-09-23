public abstract class IncrementingResource : Resource
{
    public override void Use(float value)
    {
        this.Value += value;
    }
}