using System;
using System.Collections.Generic;

public class PlayerResourceCalculator
{
    public static bool EnoughForTransportOption(TransportOption option)
    {
        foreach (KeyValuePair<Type, float> entry in option.TransportType.ResourceCostsPerDistance)
        {
            if (entry.Key != typeof(CO2Resource))
            {
                if (GetPlayer().Resources[entry.Key].Value < entry.Value)
                {
                    return false;
                }
            }

        }
        return true;
    }

    public static bool EnoughForCityStay(City city, float daysStaytime)
    {
        float costs;
        if (daysStaytime > 1)
            costs = daysStaytime * city.CostsPerNight[GetPlayer().Avatar.AvatarType];
        else
            costs = city.CostsPerDay[GetPlayer().Avatar.AvatarType];

        return EnoughMoneyAndTime(costs, daysStaytime);
    }

    public static bool EnoughForPackage(SightseeingPackage package)
    {
        float cityCosts;
        float days = package.days;
        City city = GraphGenerator.GetCity(package.cityName);

        if (days > 1)
            cityCosts = days * city.CostsPerNight[GetPlayer().Avatar.AvatarType];
        else
            cityCosts = city.CostsPerDay[GetPlayer().Avatar.AvatarType];

        float totalCosts = cityCosts + package.resources[typeof(MoneyResource)];
        return EnoughMoneyAndTime(totalCosts, days);
    }

    private static bool EnoughMoneyAndTime(float costs, float days)
    {
        return GetPlayer().Resources[typeof(MoneyResource)].Value >= costs && TimeManager.Instance.GetRemainingDays() >= days;
    }

    private static Player GetPlayer() { return GameManager.Instance.Player; }
}