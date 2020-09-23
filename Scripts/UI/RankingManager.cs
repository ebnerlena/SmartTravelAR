using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour, IObserver<Dictionary<Type, Resource>>, IObserver<int>
{
    public static RankingManager Instance { get; private set; }

    public Text rankText;
    public Text playerCountText;
    public string beforePlayerCountStr = "/";

    public Image moneyBar, co2Bar;
    public float startMoney, maxCo2;
    public Text culturePointsText;

    private Dictionary<Type, (Image,float)> bars;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        bars = new Dictionary<Type, (Image, float)>
        {
            {typeof(MoneyResource), (moneyBar, startMoney)},
            {typeof(CO2Resource), (co2Bar, maxCo2)}
        };
    }

    public void UpdateLocalScores(Dictionary<Type, Resource> resources)
    {
        if(culturePointsText != null)
        {
            culturePointsText.text = GameManager.Instance.Player.CulturePoints.ToString();
        }

        foreach(KeyValuePair<Type, Resource> entry in resources)
        {
            if(bars.ContainsKey(entry.Key))
            {
                var barEntry = bars[entry.Key];
                if(barEntry.Item1.type.Equals(Image.Type.Filled))
                    GameManager.Instance.ExecuteOnMain(() => barEntry.Item1.fillAmount = barEntry.Item2 > 0 ? entry.Value.Value / barEntry.Item2 : 0f);

                string resName = entry.Key.Name.Substring(0, entry.Key.Name.IndexOf('R'));
                string value = (entry.Value.Value > 0 ? entry.Value.Value : 0).ToString();
                string unit = entry.Value.GetUnitString();
                // "Time: 12 Stunden"
                string barTextString = string.Format("{0}: {1} {2}", resName, value, unit);

                Text barText = barEntry.Item1.GetComponentInChildren<Text>();
                if (barText != null)
                    GameManager.Instance.ExecuteOnMain(() => barText.text = barTextString);
            }
        }
    }

    public void UpdateLocalPlayerRanking()
    {
        int rank = GameManager.Instance.PlayerListManager.GetRankOfPlayer(GameManager.Instance.Player);
        if(rank >= 0 && rankText != null)
        {
            GameManager.Instance.ExecuteOnMain(() => rankText.text = rank.ToString());
        }
        int playerCount = GameManager.Instance.PlayerListManager.PlayerList.Count;
        if(playerCountText != null)
        {
            GameManager.Instance.ExecuteOnMain(() => playerCountText.text = beforePlayerCountStr + playerCount);
        }
    }

    public void ObserverUpdate(Dictionary<Type, Resource> obj)
    {
        try
        {
            GameManager.Instance.ExecuteOnMain(() => UpdateLocalScores(obj));
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void SetStartValues(Avatar avatar)
    {
        Type monType = typeof(MoneyResource);
        if (avatar.StartValues.ContainsKey(monType))
        {
            startMoney = avatar.StartValues[monType];
            if (bars.ContainsKey(monType))
                bars[monType] = (bars[monType].Item1, startMoney);
        }
    }

    public void ObserverUpdate(int obj)
    {
        UpdateLocalPlayerRanking();
    }
}
