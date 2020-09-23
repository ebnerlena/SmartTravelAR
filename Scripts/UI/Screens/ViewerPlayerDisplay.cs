using System;
using UnityEngine;
using UnityEngine.UI;

public class ViewerPlayerDisplay : MonoBehaviour
{
    public Text pName, city;
    public Image icon;
    public Image moneyBar;
    public Text moneyText, co2Text, cultureText;
    public string culturePrefix, cityPrefix;

    public void AdaptTo(EnemyPlayer player)
    {
        if (pName != null)
            pName.text = player.Name;
        
        if (city != null)
            city.text = cityPrefix + player.CurCityName;
        
        if (cultureText != null)
            cultureText.text = culturePrefix + player.CulturePoints.ToString();

        if (icon != null && player.Avatar.Icon != null)
            icon.sprite = player.Avatar.Icon;

        Type monType = typeof(MoneyResource);
        if(player.Resources.ContainsKey(monType))
        {
            float curMoney = player.Resources[monType].Value;
            float maxMoney = player.Avatar.StartValues.ContainsKey(monType) ? player.Avatar.StartValues[monType] : 1400f;
            GameManager.Instance.ExecuteOnMain(() => moneyBar.fillAmount = curMoney / maxMoney);

            string prefix = monType.Name.Substring(0, monType.Name.IndexOf('R'));
            string unit = player.Resources[monType].GetUnitString();
            string moneyTextString = string.Format("{0}: {1} {2}", prefix, curMoney, unit);
            if (moneyText != null)
                GameManager.Instance.ExecuteOnMain(() => moneyText.text = moneyTextString);
        }

        Type co2Type = typeof(CO2Resource);
        if (player.Resources.ContainsKey(co2Type))
        {
            string prefix = co2Type.Name.Substring(0, co2Type.Name.IndexOf('R'));
            string unit = player.Resources[co2Type].GetUnitString();
            string co2TextString = string.Format("{0}: {1} {2}", prefix, player.Resources[co2Type].Value, unit);
            if (co2Type != null)
                GameManager.Instance.ExecuteOnMain(() => co2Text.text = co2TextString);
        }
    }
}
