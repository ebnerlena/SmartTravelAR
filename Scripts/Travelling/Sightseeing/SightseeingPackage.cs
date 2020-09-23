using System;
using System.Collections.Generic;

public class SightseeingPackage
{
    public int days { get; private set; }
    public string cityName { get; private set; }
    public string title { get; private set; }
    public string description { get; private set; }
    public string prefabName { get; private set; }
    public float culturePoints { get; private set; }

    public Dictionary<Type, float> resources = new Dictionary<Type, float>();
    // season: summer / winter

    public SightseeingPackage(int days, string cityName, string title, string description, string prefabName, 
        float moneyCost, float co2Cost, float culturePoints)
    {
        this.days = days;
        this.cityName = cityName;
        this.title = title;
        this.description = description;
        this.prefabName = prefabName;
        this.culturePoints = culturePoints;

        //moneycost without overnight accommodation costs
        this.resources = DictionaryStripper.GetResourcesDictionary(moneyCost, co2Cost);
    }
}
