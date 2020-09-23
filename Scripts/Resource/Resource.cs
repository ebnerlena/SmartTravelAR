public abstract class Resource
{
    public string Name { get; protected set; }
    public float Value { get; set; }

    public abstract void Use(float value);

    public abstract string GetUnitString();
}
