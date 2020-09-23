using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Avatar
{
    public string Name { get; }
    public AvatarType AvatarType { get; }
    public Sprite Icon { get; }
    public float MinutesToPlay { get; }
    public Dictionary<Type, float> StartValues { get; }

    public Avatar(AvatarType type, string name, float minsToPlay, Sprite icon, Dictionary<Type, float> startValues)
    {
        this.StartValues = startValues;
        this.MinutesToPlay = minsToPlay;
        this.Name = name;
        this.AvatarType = type;
        this.Icon = icon;
    }
}