using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public static class GraphGenerator
{
    public static Dictionary<string,City> cities;
    private static List<TransportOption> cityConnections = new List<TransportOption>();

    private static CultureInfo deCulture = new CultureInfo("de-DE");

    static GraphGenerator()
    {
        CreateCityList();
        CreateCityConnectionGraph();
    }

    public static void CreateCityList()
    {
        //string path = "Assets/SourceFiles/städte.csv";
        TextAsset citiesFile = ResourceLoader.CitiesText();
        if (citiesFile == null)
            throw new FileNotFoundException("City resource do not exist");


        string input = citiesFile.text;
        string[] lines = input.Split('\n');
        cities = new Dictionary<string, City>();

        for (int row = 0; row < lines.Length - 1; row++)
        {
            string[] columns = lines[row].Split(';');
            cities.Add(columns[0],
                new City(columns[0], //name
                    float.Parse(columns[1], deCulture), //culture pro 12h
                    float.Parse(columns[2], deCulture), //student pro 24h
                    float.Parse(columns[3], deCulture), //student pro 12h
                    float.Parse(columns[4], deCulture), //businessman pro 24h
                    float.Parse(columns[5], deCulture))); //businessmann pro 12h
        }
    }

    public static void CreateCityConnectionGraph()
    {
        //string path = "Assets/SourceFiles/städteverbindungen.csv";
        TextAsset connections = ResourceLoader.CityConnectionsText();

        if(connections == null)
            throw new FileNotFoundException("City connections resource do not exist");

        string input = connections.text;
        string[] lines = input.Split('\n');

        for (int row = 0; row < lines.Length - 1; row++)
        {
            string[] columns = lines[row].Split(';');
    
            if (columns[2] != "")
            {
                cityConnections.Add(new TransportOption(
                       GetCity(columns[0]), //from
                       GetCity(columns[1]), //to
                       TransportOption.TRAIN,
                       float.Parse(columns[2], deCulture), //time in h
                       float.Parse(columns[4], deCulture), //co2
                       float.Parse(columns[5], deCulture), //money
                       float.Parse(columns[14], deCulture))); //distance

                cityConnections.Add(ReverseOption(cityConnections[cityConnections.Count - 1]));
            }
            
           if (columns[6] != "")
           {
                cityConnections.Add(
                 new TransportOption(
                     GetCity(columns[0]), //from
                     GetCity(columns[1]), //to
                     TransportOption.PLANE,
                     float.Parse(columns[6], deCulture), //time in h
                     float.Parse(columns[8], deCulture), //co2
                     float.Parse(columns[9], deCulture), //money
                     float.Parse(columns[14], deCulture))); //distance

                cityConnections.Add(ReverseOption(cityConnections[cityConnections.Count - 1]));
            }


            if (columns[10] != "")
           {
                cityConnections.Add(
                    new TransportOption(
                        GetCity(columns[0]), //from
                        GetCity(columns[1]), //to
                        TransportOption.CAR,
                        float.Parse(columns[10], deCulture), //time in h
                        float.Parse(columns[12], deCulture), //co2
                        float.Parse(columns[13], deCulture), //money
                        float.Parse(columns[14], deCulture))); //distance

                cityConnections.Add(ReverseOption(cityConnections[cityConnections.Count - 1]));
            }

        }
    }

    public static City GetCity(string cityName)
    {
        if (cities.ContainsKey(cityName))
            return cities[cityName];
        else
            return null;
    }

    public static List<string> GetCityOptions(City currentCity)
    {
        List<string> cities = new List<string>();

        foreach (TransportOption option in cityConnections)
        {
            if (option.From == currentCity && !cities.Contains(option.To.Name))
            {
                if (PlayerResourceCalculator.EnoughForTransportOption(option))
                    cities.Add(option.To.Name);
            }

        }
        return cities;
    }

    public static List<string> GetTransportTypes(City currentCity, string to)
    {
        List<string> types = new List<string>();
        City toCity = GetCity(to);

        foreach (TransportOption option in cityConnections)
        {
            if (option.From == currentCity && option.To == toCity && PlayerResourceCalculator.EnoughForTransportOption(option))
            {
                types.Add(option.TransportType.Name);
            }

        }
        return types;
    }

    public static List<TransportOption> GetValidTransportOptions(City currentCity, string to)
    {
        List<TransportOption> options = new List<TransportOption>();
        City toCity = GetCity(to);

        foreach (TransportOption option in cityConnections)
        {
            if (option.From == currentCity && option.To == toCity && PlayerResourceCalculator.EnoughForTransportOption(option))
            {
                options.Add(option);
            }
        }
        return options;
    }

    public static List<TransportOption> GetAll3TransportOptions(City currentCity, string to)
    {
        List<TransportOption> options = new List<TransportOption>();
        City toCity = GetCity(to);

        foreach (TransportOption option in cityConnections)
        {
            if (option.From == currentCity && option.To == toCity)
            {
                options.Add(option);
            }
        }
        return options;
    }


    public static TransportOption GetTransportOption(City from, string to, string transportType)
    {
        City toCity = GetCity(to);
        TransportOption op = null;
        foreach (TransportOption option in cityConnections)
        {
            if (option.From == from && option.To == toCity && option.TransportType.Name == transportType)
            {
                op = option;
                break;
            }
        }
        return op;
    }

    private static TransportOption ReverseOption(TransportOption option)
    {
        return new TransportOption(option.To, option.From, option.TransportType, option.Distance);
    }


}
