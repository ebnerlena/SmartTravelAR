using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InCity))]
public class InCityPackageHandler : MonoBehaviour
{
    private InCity inCity;
    private ARObjectsManager arObjectsManager;
    private GameObject packageInfoScreen;

    private SightseeingPackage chosenPackage;

    public List<SightseeingPackage> packages { get; private set; }
    public List<GameObject> packageObjects;
    public GameObject packageParent;

    #pragma warning disable 0649
    [SerializeField]
    private Text moneyResVal, timeResVal, co2ResVal, cultureResVal;
    
    private readonly string dayPassedEventNameTrail = "daypassed";

    void Awake()
    {
        packages = new List<SightseeingPackage>();
    }

    void Start()
    {
        inCity = GetComponent<InCity>();

        var config = inCity.GetPackageHanlderConfig();
        packageInfoScreen = config.packageInfoScreen;
        arObjectsManager = config.Item1;
    }

    public void SetupPackages()
    {
        packages.Clear();

        // add as many packages as there are parents
        for(int day = 1; day <= packageObjects.Count; day++)
        {
            SightseeingPackage pack = SightseeingManager.GetPackage(day, inCity.curCity.Name);
            packages.Add(pack);

            // fill out title and description for current package
            Text[] textFields = packageObjects[day-1].GetComponentsInChildren<Text>();
            textFields[0].text = pack.title;
            textFields[1].text = pack.days + (pack.days > 1 ? " Tage\n" : " Tag\n");
            textFields[1].text += pack.resources[typeof(CO2Resource)] + "g CO2\n";
            textFields[1].text += pack.resources[typeof(MoneyResource)] + ",- EUR\n";
            textFields[1].text += pack.culturePoints + " Wissenspunkte";
        }
 
        SetupPackagesButton();
    }

    private void SetupPackagesButton()
    {
        bool hasValidOptions = false;

        for (int pIdx = 0; pIdx < packageObjects.Count; pIdx++)
        {
            Button btn = packageObjects[pIdx].GetComponentInChildren<Button>();
            if (PlayerResourceCalculator.EnoughForPackage(packages[pIdx]))
            {
                btn.interactable = true;
                hasValidOptions = true;
            }
            else
                btn.interactable = false;
        }

        if (!hasValidOptions)
            GameManager.Instance.GameOver();
    }

    public void SetupEvents(TimeEventGroup eventGroup)
    {
        for (int day = 1; day <= 3; day++)
        {
            int curDayCopy = day;
            eventGroup.RegisterEvent(new TimeEvent(day + dayPassedEventNameTrail, () => OnDaysPassed(curDayCopy), (float)TimeManager.CityDaysToGameSec(day), false));
        }
    }

    public void OnDaysPassed(int days)
    {
        Debug.Log("days passed " + days);
        // get indices of packages where days match passed days
        foreach (int pIdx in packages.AsQueryable().Where(p => p.days == days).Select(p => packages.IndexOf(p)))
        {
            if (pIdx < packageObjects.Count)
            {
                Button selectBtn = packageObjects[pIdx].GetComponentInChildren<Button>();
                if(selectBtn != null)
                    selectBtn.interactable = false;
            }
        }
    }

    public void ShowPackageInfoScreen(int packageIdx)
    {
        packageIdx--;
        if (packageIdx >= 0 && packageIdx < packages.Count)
            chosenPackage = packages[packageIdx];
        else
            return;

        Text[] textFields = packageInfoScreen.GetComponentsInChildren<Text>();
        textFields[0].text = chosenPackage.title;
        textFields[1].text = chosenPackage.description;
        moneyResVal.text = chosenPackage.resources[typeof(MoneyResource)] + " EUR";
        timeResVal.text = chosenPackage.days + (chosenPackage.days > 1 ? " Tage" : " Tag");
        co2ResVal.text = chosenPackage.resources[typeof(CO2Resource)] + "g";
        cultureResVal.text = chosenPackage.culturePoints.ToString();

        Button [] buttons = packageInfoScreen.GetComponentsInChildren<Button>();

        if (PlayerResourceCalculator.EnoughForPackage(chosenPackage))
            buttons[1].interactable = true;
        else
            buttons[1].interactable = false;

        inCity.ToPackageInfoScreen();
    }

    public void ChoosePackage()
    {
        inCity.eventGroup?.AdaptTotalTime((float)TimeManager.CityDaysToGameSec(chosenPackage.days));
        inCity.AddCityStayInCurrent(chosenPackage);

        // cancel events where packages get disabled
        for (int day = 1; day <= 3; day++)
        {
            inCity.eventGroup?.CancelEvent(day + dayPassedEventNameTrail);
        }

        if (MobileOnlyActivator.IsMobile)
            arObjectsManager.ChooseObject(chosenPackage.days);
    }

    public void ActivateARIfMobile()
    {
        if (MobileOnlyActivator.IsMobile)
        {
            packageParent.SetActive(false);
            arObjectsManager.Activate();
        }
    }
}