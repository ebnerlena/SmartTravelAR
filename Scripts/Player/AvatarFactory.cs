using System.Collections.Generic;
using System;
using UnityEngine;

public static class AvatarFactory
{
    private static Dictionary<AvatarType, Sprite> icons;
    static AvatarFactory()
    {
        icons = new Dictionary<AvatarType, Sprite>
        {
            { AvatarType.Student, ResourceLoader.Avatar(AvatarType.Student) },
            { AvatarType.Businessman, ResourceLoader.Avatar(AvatarType.Businessman) }
        };
    }

    public static Avatar CreateTypeOf(AvatarType type)
    {
        Dictionary<Type, float> startValues = new Dictionary<Type, float>();
        string name = "none";
        float minsToPlay = TimeManager.defaultMinsToPlay;

        if (type.Equals(AvatarType.Student))
        {
            name = "Student";
            minsToPlay = 15;
            startValues.Add(typeof(MoneyResource), 1700);
            startValues.Add(typeof(CO2Resource), 0);
        }

        else if (type.Equals(AvatarType.Businessman))
        {
            name = "Businessman";
            minsToPlay = 12; // 12min to play
            startValues.Add(typeof(MoneyResource), 2800);
            startValues.Add(typeof(CO2Resource), 0);
        }

        Sprite icon = null;
        if(icons.ContainsKey(type))
        {
            icon = icons[type];
        }

        return new Avatar(type, name, minsToPlay, icon, startValues);
    }
}
