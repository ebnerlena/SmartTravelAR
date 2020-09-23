using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportOption
{
    public const string TRAIN = "TRAIN";
    public const string PLANE = "PLANE";
    public const string CAR = "CAR";

    public City From { get; }

    public City To { get; }

    public TransportType TransportType { get; }

    public float Distance { get; }

    public TransportOption(City from, City to, string transportType, float time, float co2, float money, float distance)
    {
        this.From = from;
        this.To = to;
        this.Distance = distance;

        //maybe make transporttypefactory
        switch (transportType)
        {
            case TRAIN:
                this.TransportType = new TrainTransport(time, money,co2); 
                break;
            case PLANE:
                this.TransportType = new PlaneTransport(time, money, co2);
                break;
            case CAR:
                this.TransportType = new CarTransport(time, money, co2); 
                break;
        }
    }

    public TransportOption(City from, City to, TransportType type, float distance)
    {
        this.From = from;
        this.To = to;
        this.TransportType = type;
        this.Distance = distance;
    }
}
