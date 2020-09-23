using UnityEngine;
using System.Collections.Generic;

public class City
{
    public string Name { get; set; }

    public GameObject Model { get; set; }

    public string ARid { get; set; }

    public float CulturePointsPer12Hrs { get; }

    public Dictionary<AvatarType, float> CostsPerDay = new Dictionary<AvatarType, float>();

    public Dictionary<AvatarType, float> CostsPerNight = new Dictionary<AvatarType, float>();

    public City(string name, float culture, float s_costsPerNight, float s_costsPerDay, float b_costsPerNight, float b_costsPerDay)
    {
        this.Name = name;
        this.CulturePointsPer12Hrs = culture;

        this.CostsPerNight.Add(AvatarType.Student, s_costsPerNight);
        this.CostsPerNight.Add(AvatarType.Businessman, b_costsPerNight);

        this.CostsPerDay.Add(AvatarType.Student, s_costsPerDay);
        this.CostsPerDay.Add(AvatarType.Businessman, b_costsPerDay);        
    }
}
