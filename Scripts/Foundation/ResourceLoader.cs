using System;
using UnityEngine;

public static class ResourceLoader
{
    private static readonly string avatarPath = "Images/Icons/Avatars/";
    private static readonly string transportTypePath = "Images/Icons/Transports/";
    private static readonly string citiesPath = "CityCSV/cities";
    private static readonly string cityConnectionsPath = "CityCSV/cityconnections";
    private static readonly string sightseeingPackagesPath = "SightseeingPackages/";
    private static readonly string routesPath = "Images/Routen/";

    public static Sprite Avatar(string name)
    {
        string spritePath = avatarPath + name;
        return Resources.Load<Sprite>(spritePath);
    }

    public static Sprite Avatar(Avatar avatar)
    {
        if (avatar != null)
            return Avatar(avatar.Name);
        else
            return null;
    }

    public static Sprite Avatar(AvatarType type)
    {
        return Avatar(type.ToString());
    }

    public static Sprite TransportType(string name)
    {
        return Resources.Load<Sprite>(transportTypePath + name);
    }

    /*public static Sprite TransportType(Type type)
    {
        if(TypeHelper.IsTypeOf<TransportType>(type))
        {
            return TransportType(TypeHelper.ToString(type));
        }
        return null;
    }*/

    public static Sprite TransportType(TransportType type)
    {
        string typeName = TypeHelper.ToString(type.GetType());
        return Resources.Load<Sprite>(transportTypePath + typeName.Remove(typeName.IndexOf("Transport")));
    }

    public static TextAsset CitiesText()
    {
        return Resources.Load<TextAsset>(citiesPath);
    }

    public static TextAsset CityConnectionsText()
    {
        return Resources.Load<TextAsset>(cityConnectionsPath);
    }

    public static TextAsset SightSeeingPackages()
    {
        return Resources.Load<TextAsset>(sightseeingPackagesPath+"packages");
    }

    public static Sprite RouteMap(string from, string to)
    {
        Sprite found = null;
        found  = Resources.Load<Sprite>(routesPath + from+"-"+to);

        if (found == null)
            found = Resources.Load<Sprite>(routesPath + to +"-"+ from);

        return found;

    }

    public static GameObject PackagePrefab(string prefabName)
    {
        return Resources.Load<GameObject>(sightseeingPackagesPath + prefabName).gameObject;
    }
}