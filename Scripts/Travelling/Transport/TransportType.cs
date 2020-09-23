using System.Collections.Generic;
using System;

public abstract class TransportType
{
    public string Name { get; protected set; }

    // todo: look
    public Dictionary<Type, float> ResourceCostsPerDistance { get; }

    public float TimeInHours { get; private set; }

    public TransportType(float time, float money, float co2)
    {
        this.ResourceCostsPerDistance = new Dictionary<Type, float>();
        this.TimeInHours = time;

        this.ResourceCostsPerDistance.Add(typeof(MoneyResource), money); //in €
        this.ResourceCostsPerDistance.Add(typeof(CO2Resource), co2); //in co2/kg
    }

}