using System;

public class Transport
{
    public CityStay From { get; }

    public CityStay To { get; }

    public TransportOption Option { get; }

    public DateTime StartTime { get; private set; }

    public float MaxTimeInSeconds { get; }

   
    public Transport(TransportOption option, float maxTimeSeconds)
    {
        this.From = new CityStay(option.From); //+ staytime default 1 day
        this.To = new CityStay(option.To); 
        this.Option = option;
        this.MaxTimeInSeconds = maxTimeSeconds;
    }

    public void Start ()
    {
        this.StartTime = DateTime.UtcNow;
    }

    public bool StartMinigame()
    {
        return true;
    }
}
