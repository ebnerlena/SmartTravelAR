public class PlaneTransport : TransportType
{

    public PlaneTransport(float time, float money, float co2)
        : base (time, money, co2)
    {
        this.Name = "Plane";
    }
}
