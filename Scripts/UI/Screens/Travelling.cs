using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class Travelling : Screen
{
#pragma warning disable 0649
    [SerializeField]
    private Image transportTypeImage;

    [SerializeField]
    private Text timerText;

    [SerializeField]
    private Text fromTo, scanCueCity;
#pragma warning restore 0649

    public int initImageSearchAtSecondsRemain;

    private bool minigameSuccess;
    private bool minigamePlaying;

    private ImageRecognizer imageRecognizer;
    private string searchName;

    private readonly string eventGroupName = "travelling";
    private TimeEventGroup eventGroup;

    private bool arrivedInTime;
    private float t;
    private float timerUpdate = 0.25f;

    void Start()
    {
        imageRecognizer = FindObjectOfType<ImageRecognizer>(); 
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t > timerUpdate && eventGroup != null)
        {
            timerText.text = eventGroup.GetRemainingSeconds().ToString("00");
        }
    }

    //called when transport model is pressed
    public void CityCheckIn()
    {
        if (MobileOnlyActivator.IsMobile)
            imageRecognizer.enabled = false;

        if (GameManager.Instance.Player.Trip.IsCompleted())
            GameManager.Instance.GameOver();
        else
        {
            GameManager.Instance.Status = GameStatus.InCity;
            if (!minigameSuccess || !arrivedInTime)
                GameManager.Instance.SendCancelTripUpdate();
        }
    }

    public override void Show()
    {
        base.Show();
        arrivedInTime = true;
        imageRecognizer = FindObjectOfType<ImageRecognizer>();
        Trip trip = GameManager.Instance.Player.Trip;
        fromTo.text = trip.CurrentCity.Name + " - " + trip.CurrentTransport.Option.To.Name;
        LoadTransportIcon();

        SetupEventGroup();
        eventGroup.Start();

        if (eventGroup.GetRemainingSeconds() > 20)
        {
            MinigameController.Instance.LoadGame();
            minigameSuccess = false;
            minigamePlaying = true;
        }
        else
        {
            minigamePlaying = false;
            minigameSuccess = true;
        }
    }

    // called by button instead of scanning correct image
    public void ArrivedInCityManually()
    {
        GameManager.Instance.Player.Trip.ArrivedInTime();
        MinigameController.Instance.StopCurrentGame();
        TimeManager.Instance.CancelEventGroup(eventGroupName);

        CityCheckIn();
    }

    //OnImageFound
    public void ArrivedInSearchedCity()
    {
        if(TimeManager.Instance.Find(eventGroupName) != null)
        {
            arrivedInTime = eventGroup.GetRemainingSeconds() >= 0;
            if ((arrivedInTime && minigameSuccess) || (arrivedInTime && !minigamePlaying))
                GameManager.Instance.Player.Trip.ArrivedInTime();

            TimeManager.Instance.CancelEventGroup(eventGroupName);
        }
    }

    //prepare for imagesearching
    private void On10SecondsRemain()
    {
        if (MobileOnlyActivator.IsMobile)
        {
            Handheld.Vibrate();
        }
        
        if (minigamePlaying)
        {
            MinigameController.Instance.StopCurrentGame();
            minigameSuccess = MinigameController.Instance.GetSuccess();    
        }

        if (!minigameSuccess)
        {
            GameManager.Instance.Player.Trip.Cancel();
            eventGroup.CancelEvent("absoluteEnd");
            searchName = GameManager.Instance.Player.Trip.CurrentCity.Name;
        }
        else
            searchName = GameManager.Instance.Player.Trip.CurrentTransport.To.City.Name;


        if (MobileOnlyActivator.IsMobile)
        {
            scanCueCity.text = searchName;
            imageRecognizer.enabled = true;
            imageRecognizer.SearchForImage(searchName, ArrivedInSearchedCity);
        }
              
    }

    //end of travelling time - chancel trip, back to start city
    private void OnAbsoluteEnd()
    {
        GameManager.Instance.Player.Trip.Cancel();

        if (MobileOnlyActivator.IsMobile)
        {
            Handheld.Vibrate();
            searchName = GameManager.Instance.Player.Trip.CurrentCity.Name;
            scanCueCity.text = searchName;
            imageRecognizer.enabled = true;
            imageRecognizer.SearchForImage(searchName, ArrivedInSearchedCity);
        }

        GameManager.Instance.SetErrorMessage(ErrorMessageType.TravellingError, "zu langsam...");
    }



    private void SetupEventGroup()
    {
        TimeManager.Instance.CancelEventGroup(eventGroupName);

        eventGroup = new TimeEventGroup(eventGroupName, GameManager.Instance.Player.Trip.CurrentTransport.MaxTimeInSeconds);
        TimeEvent endEvent = new TimeEvent("absoluteEnd", OnAbsoluteEnd, 0f, true);
        TimeEvent tenSecRemainEvent = new TimeEvent("tenSecRemain", On10SecondsRemain, Mathf.Max(5f, initImageSearchAtSecondsRemain), true);
        eventGroup.RegisterEvent(endEvent);
        eventGroup.RegisterEvent(tenSecRemainEvent);

        TimeManager.Instance.RegisterEventGroup(eventGroup);
    }

    private void LoadTransportIcon()
    {
        TransportType type = GameManager.Instance.Player.Trip.CurrentTransport.Option.TransportType;
        Sprite icon = ResourceLoader.TransportType(type);
        if (icon != null)
            transportTypeImage.sprite = icon;
    }

    public override void Hide()
    {
        base.Hide();
        TimeManager.Instance.CancelEventGroup(eventGroupName);
    }

}
