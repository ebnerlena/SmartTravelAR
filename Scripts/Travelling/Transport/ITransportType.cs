using System.Collections.Generic;
using System;

public interface ITransportType
{
    string Name { get;  set; }

    Dictionary<Type, float> ResourceCostsPerDistance { get; set; }

    float InAppTimePerDistance { get; set; }
}
