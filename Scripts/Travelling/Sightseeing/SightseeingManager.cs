using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightseeingManager
{
    private static List<SightseeingPackage> packages = SightseeingParser.Parse();
    private static System.Random rnd = new System.Random();

    public static SightseeingPackage GetPackage(int days, string cityName)
    {
        List<SightseeingPackage> possiblePackages = packages.FindAll(p => p.days == days && p.cityName.Equals(cityName));
        int choice = rnd.Next(possiblePackages.Count);

        //Debug.Log("rnd has choosen " + choice + " package: " + possiblePackages[choice].title);
        return possiblePackages[choice];
    }
}
