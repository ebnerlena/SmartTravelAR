public abstract class DecrementingResource : Resource
{
    public override void Use(float value)
    {
        this.Value -= value;
    }
}