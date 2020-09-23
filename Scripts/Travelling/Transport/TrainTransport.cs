public class TrainTransport : TransportType
{
    public TrainTransport(float time, float money, float co2)
       : base(time, money, co2)
    {
        this.Name = "Train";
    }
}
