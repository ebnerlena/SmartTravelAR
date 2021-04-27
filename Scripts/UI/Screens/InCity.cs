using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InCityPackageHandler))]
public class InCity : Screen
{
#pragma warning disable 0649
    private string selectedCity, selectedTransport;

    [SerializeField]
    private GameObject cityChoiceScreen, packageChoiceScreen, transportChoiceScreen, packageInfoScreen, mapMask, resources, cityButtons, cityButtonPrefab;

    [SerializeField]
    private GameObject trainTransport, planeTransport, carTransport;

    [SerializeField]
    private Text packagesCurrentCity, username, nextStopCurrentCity, fromTo;

    [SerializeField]
    private Image avatarIcon, routeMap;

    [SerializeField]
    private Text timerText;
    public float cityOptionsTime = 20f;

    private Text transportInfo;
#pragma warning restore 0649

    private Button chooseTransportButton;
    private List<TransportOption> allOptions;
    private List<TransportOption> validOptions;
    private ARObjectsManager arObjectsManager;
    public City curCity { get; private set; }
    private InCityPackageHandler packageHandler;

    public TimeEventGroup eventGroup { get; private set; }
    private readonly string eventGroupName = "incity";
    private float t;
    private float timerUpdate = 0.5f;
    private System.Random rnd = new System.Random();

    void Awake()
    {
        screens.Add(cityChoiceScreen);
        screens.Add(packageChoiceScreen);
        screens.Add(packageInfoScreen);
        screens.Add(transportChoiceScreen);

        validOptions = new List<TransportOption>();
        allOptions = new List<TransportOption>();
    }

    void Start()
    {
        packageHandler = GetComponent<InCityPackageHandler>();
        SetupTopbar();

        if (MobileOnlyActivator.IsMobile)
        {
            arObjectsManager = FindObjectOfType<ARObjectsManager>();
            arObjectsManager.Deactivate();
        }
    }

    void Update()
    {
        t += Time.deltaTime;
        if(t > timerUpdate && eventGroup != null)
        {
            timerText.text = eventGroup.GetRemainingSeconds().ToString("00");
        }
    }

    #region Trip related
    public void AddCityStayInCurrent(SightseeingPackage package)
    {
        City city = GameManager.Instance.Player.Trip.CurrentCity;
        if (PlayerResourceCalculator.EnoughForCityStay(city, package.days))
        {
            GameManager.Instance.Player.AddCityStayWithPkg(city, package);

            if (MobileOnlyActivator.IsMobile)
            {
                ClosePackageInfoScreen();
            }

            else
            {
                DisplayCityChoiceScreen();
                UpdateCity();
            }
        }
        else
            GameManager.Instance.SetErrorMessage(ErrorMessageType.ResourcesError, "not enough resources");
    }
    private void ChooseCity(string city)
    {
        selectedCity = city;
        DisplayTransportChoiceScreen();
    }

    //Continue trip
    public void ChooseTransport(string type)
    {
        TimeManager.Instance.CancelEventGroup(eventGroupName);
        selectedTransport = type;
        GameManager.Instance.StartTrip(selectedCity, selectedTransport);
    }

    #endregion

    #region UI Setup
    private void SetupTopbar()
    {
        this.username.text = GameManager.Instance.Player.Name;
        Sprite icon = GameManager.Instance.Player?.Avatar?.Icon;
        if (icon != null)
            this.avatarIcon.sprite = icon;
    }


    private void SetupCityButtons()
    {
        var children = new List<GameObject>();
        foreach (Transform child in cityButtons.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        List<string> cities = GraphGenerator.GetCityOptions(curCity);

        if (cities.Count > 0)
        {
            float yOffset = 0.0f;
            Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
            foreach (string city in cities)
            {
                GameObject button = (GameObject)Instantiate(cityButtonPrefab);
                button.transform.SetParent(cityButtons.transform);
                button.transform.localScale = scale;
                var t = button.GetComponent<RectTransform>();

                Vector2 newAnchorsMin = new Vector2(t.anchorMin.x, t.anchorMin.y - yOffset);
                Vector2 newAnchorsMax = new Vector2(t.anchorMax.x, t.anchorMax.y - yOffset);

                t.anchorMin = newAnchorsMin;
                t.anchorMax = newAnchorsMax;
                t.offsetMin = t.offsetMax = new Vector2(0, 0);

                yOffset += 0.25f;
                button.GetComponent<Button>().onClick.AddListener(() => ChooseCity(city));
                button.transform.GetChild(0).GetComponent<Text>().text = city;
            }
        }
        else
            GameManager.Instance.GameOver();
    }

    #endregion

    #region Setup - TransportTypes
    private void SetupTransportTypes()
    {
        City curCity = GameManager.Instance.Player.Trip.CurrentCity;
        fromTo.text = curCity.Name + " - " + selectedCity;
        validOptions = GraphGenerator.GetValidTransportOptions(curCity, selectedCity);
        allOptions = GraphGenerator.GetAll3TransportOptions(curCity, selectedCity);
        LoadRouteMap(curCity.Name);
        LoadTransportTypeInfo();
        SetupAvailableTransportButtons();
    }

    private void LoadRouteMap(string currentCity)
    {
        Sprite map = ResourceLoader.RouteMap(currentCity, selectedCity);
        if (map != null)
            routeMap.sprite = map;
    }

    private void LoadTransportTypeInfo()
    {
        //to empy options that do not exist
        if (allOptions.Count < 3)
        {
            List<string> emptyOptions = GetEmptyOptionsTransports();
            foreach(string transportType in emptyOptions)
            {
                transportInfo = GetTransportGameObject(transportType).GetComponentInChildren<Text>();
                transportInfo.text = "";
                chooseTransportButton = GetTransportGameObject(transportType).GetComponentInChildren<Button>();
                chooseTransportButton.interactable = false;
            }
        }

        //setting up all existing option
        foreach (TransportOption option in allOptions)
        {
            transportInfo = GetTransportGameObject(option.TransportType.Name).GetComponentInChildren<Text>();

            transportInfo.text = option.TransportType.ResourceCostsPerDistance[typeof(CO2Resource)] + " kg CO2\n";
            transportInfo.text += option.TransportType.ResourceCostsPerDistance[typeof(MoneyResource)] + " EUR\n";
            transportInfo.text += "Zeit " + option.TransportType.TimeInHours + "h";

            chooseTransportButton = GetTransportGameObject(option.TransportType.Name).GetComponentInChildren<Button>();
            chooseTransportButton.interactable = false;
        }
    }

    //make options where not enough resources not interactable
    private void SetupAvailableTransportButtons()
    {
        foreach (TransportOption option in validOptions)
        {
            chooseTransportButton = GetTransportGameObject(option.TransportType.Name).GetComponentInChildren<Button>();
            chooseTransportButton.interactable = true;
        }
    }

    //get transportTypes of not existing options
    private List<string> GetEmptyOptionsTransports()
    {
        List<string> empyOptions = new List<string>();
        foreach (Transports t in Enum.GetValues(typeof(Transports)))
        {
            if(!allOptions.Exists(opt => opt.TransportType.Name == t.ToString()))
                empyOptions.Add(t.ToString());
        }
        return empyOptions;
    }

    private GameObject GetTransportGameObject(string option)
    {
        GameObject transportGameObject = null;

        switch (option)
        {
            case "Plane":
                transportGameObject = planeTransport;
                break;
            case "Train":
                transportGameObject = trainTransport;
                break;
            case "Car":
                transportGameObject = carTransport;
                break;
        }
        return transportGameObject;
    }

    #endregion

    #region Screen changes
    public void ClosePackageInfoScreen()
    {
        ChangeScreen(packageChoiceScreen);

        if (MobileOnlyActivator.IsMobile)
            arObjectsManager.isActive = true;
    }

    public void DisplayPackageChoiceScreen()
    {
        packageHandler.SetupPackages();
        StartTimeEvents(true);

        ChangeScreen(packageChoiceScreen);

        packageHandler.ActivateARIfMobile();
    }

    public void DisplayCityChoiceScreen()
    {
        try {
            // null reference probably bc of this
            if (MobileOnlyActivator.IsMobile)
                arObjectsManager.Deactivate();

            eventGroup.CancelEvent("chooseNextTravel");
            SetupCityButtons();
            ChangeScreen(cityChoiceScreen);
        }
        catch (Exception e)
        {
             DebugText.SetText(1, "11 " + e.Message);
        }
        
    }

    private void DisplayTransportChoiceScreen()
    {
        SetupTransportTypes();
        ChangeScreen(transportChoiceScreen);  
    }
    public void ToPackageInfoScreen()
    {
        ChangeScreen(packageInfoScreen);
    }
    public void ShowPackageInfoScreen(int packageDays)
    {
        packageHandler.ShowPackageInfoScreen(packageDays);

        if (MobileOnlyActivator.IsMobile)
            arObjectsManager.isActive = false;
    }
    protected override void ChangeScreen(GameObject newScreen)
    {
        base.ChangeScreen(newScreen);
    }

    private void UpdateCity()
    {
        curCity = GameManager.Instance.Player.Trip.CurrentCity;
        this.packagesCurrentCity.text = curCity.Name;
        this.nextStopCurrentCity.text = curCity.Name;
    }

    #endregion
    public override void Show()
    {
        base.Show();
        SetupTopbar();
        UpdateCity();
        mapMask.SetActive(true);
        resources.SetActive(true);
        
        if (GameManager.Instance.Player.Trip.HasAlreadyVistedCurrentCity())
        {
            StartTimeEvents(false);
            try
            {
                DisplayCityChoiceScreen();
            }
            catch (Exception e)
            {
                DebugText.SetText(1, "2 " + e.Message);
            }
        }
        else
            DisplayPackageChoiceScreen();
    }

    public override void Hide()
    {
        base.Hide();

        if (MobileOnlyActivator.IsMobile)
            arObjectsManager.Deactivate();
    }

    #region Time Events
    private void ShowCityOptions()
    {
        DisplayCityChoiceScreen();
        UpdateCity();
    }

    private void ForceTravel()
    {
        string[] transports = new string[]{"Car", "Plane", "Train"};
        string[] cities = GraphGenerator.GetCities();
        string randomCity = cities[rnd.Next(0,cities.Length)];
        string randomTansport = transports[rnd.Next(0,transports.Length)];
        GameManager.Instance.StartTrip(randomCity, randomTansport);
    }

    private void StartTimeEvents(bool withPackages)
    {
        TimeManager.Instance.CancelEventGroup(eventGroupName);
        
        float maxStaySeconds = cityOptionsTime;
        if (withPackages)
        {
            int maxDays = packageHandler.packages.AsQueryable().Select(p => p.days).Max();
            maxStaySeconds = (float)TimeManager.CityDaysToGameSec(maxDays) + cityOptionsTime;
        }

        eventGroup = new TimeEventGroup(eventGroupName, maxStaySeconds);
        eventGroup.RegisterEvent(new TimeEvent("forceTravel", ForceTravel, 0, true));

        if (withPackages)
        {
            eventGroup.RegisterEvent(new TimeEvent("chooseNextTravel", ShowCityOptions, cityOptionsTime, true));
            packageHandler.SetupEvents(eventGroup);
        }

        TimeManager.Instance.RegisterEventGroup(eventGroup);
        eventGroup.Start();
    }
    #endregion

    public (ARObjectsManager, GameObject packageInfoScreen) GetPackageHanlderConfig()
    {
        return (arObjectsManager, packageInfoScreen);
    } 
}
