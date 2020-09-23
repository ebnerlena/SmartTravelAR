using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public static class SightseeingParser
{
    private static CultureInfo deCulture = new CultureInfo("de-DE");

    public static List<SightseeingPackage> Parse()
    {
       
        TextAsset packagesText = ResourceLoader.SightSeeingPackages();

        if (packagesText == null)
            throw new FileNotFoundException("Packages resource do not exist");


        string input = packagesText.text;
        string[] lines = input.Split('\n');
        List<SightseeingPackage> packages = new List<SightseeingPackage>();
        
        for (int row = 0; row < lines.Length - 1; row++)
        {
            string[] columns = lines[row].Split(';');
            packages.Add(
                new SightseeingPackage(
                    Int32.Parse(columns[0], deCulture), //days
                    columns[1], //cityname
                    columns[2], //title
                    columns[3], //description
                    columns[4], //prefabName
                    float.Parse(columns[6], deCulture), //money
                    float.Parse(columns[7], deCulture), //co2
                    float.Parse(columns[8], deCulture))); //culture
        }
        return packages;
    }
}
