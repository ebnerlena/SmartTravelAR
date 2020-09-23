using System;
using System.Collections.Generic;

public static class DictionaryStripper
{
    public static void ExtractWeightDic(Dictionary<Type, float> weightDic, out string[] resourceNames, out float[] weights)
    {
        resourceNames = new string[weightDic.Count];
        weights = new float[weightDic.Count];

        int idx = 0;
        foreach (KeyValuePair<Type, float> entry in weightDic)
        {
            resourceNames[idx] = TypeHelper.ToString(entry.Key);
            weights[idx] = entry.Value;
            idx++;
        }
    }

    public static void ExtractResourcesDic(Dictionary<Type, Resource> resources, out string[] resourceNames, out float[] rawValues)
    {
        resourceNames = new string[resources.Count];
        rawValues = new float[resources.Count];

        int idx = 0;
        foreach (KeyValuePair<Type, Resource> entry in resources)
        {
            resourceNames[idx] = TypeHelper.ToString(entry.Key);
            rawValues[idx] = entry.Value.Value;
            idx++;
        }
    }

    public static Dictionary<Type,float> GetResourcesDictionary(float money, float co2)
    {
        Dictionary<Type, float> res = new Dictionary<Type, float>();
        res.Add(typeof(MoneyResource), money);
        res.Add(typeof(CO2Resource), co2);

        return res;
    }

    public static float GetAvatarCityCostsPerDay(City city, Avatar avatar)
    {
        return city.CostsPerDay[avatar.AvatarType]; // (AvatarType)Enum.Parse(typeof(AvatarType), avatar.Name)]; what?
    }

    public static float GetAvatarCityCostsPerNight(City city, Avatar avatar)
    {
        return city.CostsPerNight[avatar.AvatarType]; // (AvatarType)Enum.Parse(typeof(AvatarType), avatar.Name)];
    }
}