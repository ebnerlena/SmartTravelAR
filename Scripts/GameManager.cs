using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IObservable<GameStatus>
{
    public static GameManager Instance { get; private set; }

    private GameStatus _status;
    public GameStatus Status {
        get => _status; 
        set
        {
            _status = value;
            StatusChanged(value);
        }
    }

    public Player Player { get; private set; }
    public NetworkPlayer NetworkPlayer { get; private set; }

    private Queue<Action> mainThreadActions;

#pragma warning disable 0649
    [SerializeField]
    private float validTripFinishPoints = 10;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private List<string> cityNames;
    [SerializeField]
    private List<GameObject> cityModels;
    public ServerChoiceParent serverChoiceParent;
   
#pragma warning restore 0649

    public PlayerListManager PlayerListManager { get; private set; }
    public List<IObserver<GameStatus>> observers { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        observers = new List<IObserver<GameStatus>>();
        mainThreadActions = new Queue<Action>();
        PlayerListManager = new PlayerListManager();

        
        SetupUI();
        SetupPlayer();

    }

    void Start()
    {
        if (MobileOnlyActivator.IsMobile)
            CameraController.Instance.ShowFirstPersonView();   
        else
            CameraController.Instance.ShowDefaultCamera();
    }

    void Update()
    {
        if (mainThreadActions.Count > 0)
        {
            mainThreadActions.Dequeue()?.Invoke();
        }
    }

    public void StartTrip(string to, string transportType)
    {
        TransportOption option = GraphGenerator.GetTransportOption(this.Player.Trip.CurrentCity, to, transportType);

        if (PlayerResourceCalculator.EnoughForTransportOption(option))
        {
            this.Player.Trip.CurrentTransport = new Transport(option, (float)TimeManager.RealHrsToGameSec(option.TransportType.TimeInHours));
            this.Player.Trip.Start();
            this.Player.UseTransportResources();

            Transport curTrans = Player.Trip.CurrentTransport;
            TripUpdateMessegable msg = new TripUpdateMessegable(Player.playerId, curTrans.MaxTimeInSeconds, curTrans.StartTime, curTrans.From.City.Name, curTrans.To.City.Name, curTrans.Option.TransportType.Name);
            NetworkPlayer.SendAndHandle(msg);
            Status = GameStatus.Travelling;
        }
        else
        {
            SetErrorMessage(ErrorMessageType.ResourcesError, "nicht genügend Resourcen");
        }
    }

    public void SendCancelTripUpdate()
    {
        Transport curTrans = Player.Trip.CurrentTransport;
        TripUpdateMessegable msg = new TripUpdateMessegable(Player.playerId, 3f, DateTime.UtcNow, curTrans.To.City.Name, curTrans.From.City.Name, curTrans.Option.TransportType.Name);
        NetworkPlayer.SendAndHandle(msg);
    }

    public GameObject GetCityModel(string cityName)
    {
        int cityIdx = cityNames.IndexOf(cityName);
        if (cityIdx >= 0)
            return cityModels[cityIdx];
        else
            return null;
    }

    public string GetTransportType()
    {
        return Player.Trip.CurrentTransport.Option.TransportType.Name;
    }

    public void SetErrorMessage(ErrorMessageType msgType, string msg)
    {
        ExecuteOnMain(() => uiManager.SetErrorMessage(msgType, msg));
    }

    public void CreatedLobbySuccess()
    {
        this.uiManager.CreatedLobbySuccess();
    }

    public void StartGame()
    {
        if (Player.isViewer)
        {
            Status = GameStatus.Viewer;
            uiManager.SetViewerLobbyId();
            TimeManager.Instance.StartGameForViewer();
        }
            
        else
        {
            Status = GameStatus.InCity;
            TimeManager.Instance.StartGameNow();
        }
    }
    
    public void GameOver()
    {
        if (Player.Trip.IsCompleted())
            Player.ValidTripFinish(validTripFinishPoints);

        this.Status = GameStatus.Ranking;            
    }

    public void ResetGame()
    {
        Status = GameStatus.Lobby;
        NetworkPlayer.SendOnly(new ExitLobbyMessageable());
        
    }

    private void SetupPlayer()
    {
        Player = new Player(GuidCreator.New());
        Player.UpdateCurCityName("Salzburg");
        Player.Trip.CurrentCity = GraphGenerator.cities["Salzburg"];
        NetworkPlayer = new NetworkPlayer(Player.playerId, serverChoiceParent);
    }

    public void AdaptPlayer(string username, AvatarType type)
    {
        Player.Name = username;
        Player.Avatar = AvatarFactory.CreateTypeOf(type);
    }

    public void SetupPlayerOnLobbyJoin()
    {
        Player.AddObserver(RankingManager.Instance);
        PlayerListManager.AddObserver(RankingManager.Instance);
        PlayerListManager.AddObserver(Map.Instance);
        RankingManager.Instance.SetStartValues(Player.Avatar);
        Player.CreateResources();
        PlayerListManager.AddLocalPlayer(Player);
        uiManager.ShowInLobbyScreen();
    }

    private void SetupUI()
    {
        uiManager.SetPlayer(this.Player);
        uiManager.SetNWPlayer(this.NetworkPlayer);
    }

    public void StatusChanged(GameStatus newStatus)
    {
        NotifyObservers(newStatus);
    }

    private void DisconnectPlayer()
    {
        NetworkPlayer?.Disconnect();
    }

    void OnApplicationQuit()
    {
        DisconnectPlayer();
    }

    #region observer methods
    public void AddObserver(IObserver<GameStatus> observer)
    {
        if (!observers.Contains(observer)) { observers.Add(observer); }
    }

    public void RemoveObserver(IObserver<GameStatus> observer)
    {
        if (observers.Contains(observer)) { observers.Remove(observer); }
    }

    public void NotifyObservers(GameStatus message)
    {
        // not using foreach, since observers can remove themselves on execution
        // and modify observers list
        for (int i = observers.Count - 1; i >= 0; i--)
        {
            observers[i].ObserverUpdate(message);
        }
    }

    #endregion

    /// <summary>
    /// For Unity Calls like SetActive 
    /// which need to be executed on the main thread.
    /// </summary>
    /// <param name="a">The action to execute</param>
    public void ExecuteOnMain(Action a)
    {
        mainThreadActions.Enqueue(a);
    }
}
