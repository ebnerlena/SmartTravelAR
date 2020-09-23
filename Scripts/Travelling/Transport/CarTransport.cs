public class CarTransport : TransportType
{
    public CarTransport(float time, float money, float co2)
       : base(time, money, co2)
    {
        this.Name = "Car";
    }
}
