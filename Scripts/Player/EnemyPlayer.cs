using System;
using System.Collections.Generic;
using System.Linq;

public class EnemyPlayer : IObserver<bool>
{
    public string playerId;
    public string Name { get; set; }
    public Avatar Avatar { get; set; }
    public float CulturePoints { get; protected set; }
    public float CurrentScore { get; protected set; }
    public float Daysleft { get; protected set; }
    public Dictionary<Type, Resource> Resources { get; protected set; }
    public string CurCityName { get; protected set; }

    public EnemyPlayer(string playerId)
    {
        this.playerId = playerId;
        CurrentScore = 0f;
        Resources = new Dictionary<Type, Resource>();
    }

    public EnemyPlayer(string playerId, string name, string avatarType) : this(playerId)
    {
        this.Name = name;

        if (Enum.TryParse(avatarType, out AvatarType type))
        {
            Avatar = AvatarFactory.CreateTypeOf(type);
        }
    }

    public virtual void CreateResources()
    {
        if (Avatar == null || Avatar.StartValues == null)
            return;

        foreach (KeyValuePair<Type, float> entry in Avatar.StartValues)
        {
            // create a new instance of resource
            this.Resources.Add(entry.Key, (Resource)Activator.CreateInstance(entry.Key, entry.Value));
        }
    }

    public void UpdateScore(float newScore)
    {
        CurrentScore = newScore;
    }

    public void UpdateCulturePoints(float culturePoints)
    {
        this.CulturePoints = culturePoints;
    }

    public void UpdateCurCityName(string curCityName)
    {
        this.CurCityName = curCityName;
    }

    public void UpdateMoneyAndCo2(string[] resNames, float[] values)
    {
        UpdateResource(typeof(MoneyResource), resNames, values);
        UpdateResource(typeof(CO2Resource), resNames, values);
    }

    private void UpdateResource(Type resType, string[] resNames, float[] values)
    {
        int idx = Array.IndexOf(resNames, TypeHelper.ToString(resType));
        if (idx >= 0 && Resources.ContainsKey(resType) && idx < values.Length)
        {
            Resources[resType].Value = values[idx];
        }
    }

    public PlayerInfo GetInfo()
    {
        return new PlayerInfo(playerId, Name, Enum.GetName(typeof(AvatarType), Avatar.AvatarType));
    }

    public void Reset()
    {
        Resources.Clear();
        CulturePoints = 0;
        CurrentScore = 0;
        UpdateCurCityName("Salzburg");
    }

    public void ObserverUpdate(bool shouldReset)
    {
        Reset();
    }
}
