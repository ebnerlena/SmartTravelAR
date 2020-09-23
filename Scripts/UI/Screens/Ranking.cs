using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : Screen, IObserver<int>, IObservable<bool>
{
#pragma warning disable 0649

    [SerializeField]
    private Text playerScore, playerRank;

    [SerializeField]
    private GameObject playerSet1, playerSet2, playerSet3, playerSet4, playerSet5, playerSet6, playerSet7;

    [SerializeField]
    private GameObject impressumScreen;
    [SerializeField]
    private GameObject rankingsScreen;

    private Dictionary<int,GameObject> playerSets = new Dictionary<int,GameObject>();
    private bool isShowing;
    public List<IObserver<bool>> observers { get; private set; }


    void Awake()
    {
        playerSets.Add(1, playerSet1);
        playerSets.Add(2, playerSet2);
        playerSets.Add(3, playerSet3);
        playerSets.Add(4, playerSet4);
        playerSets.Add(5, playerSet5);
        playerSets.Add(6, playerSet6);
        playerSets.Add(7, playerSet7);

        screens.Add(rankingsScreen);
        screens.Add(impressumScreen);
        isShowing = false;
    }

    void Start()
    {
        GameManager.Instance.PlayerListManager.AddObserver(this);
        observers = new List<IObserver<bool>>();
        this.AddObserver(GameManager.Instance.PlayerListManager);
        this.AddObserver(GameManager.Instance.Player.Trip);
        this.AddObserver(GameManager.Instance.Player);
    }

    void Update()
    {
        // Make sure user is on Android platform
        if (Application.platform == RuntimePlatform.Android)
        {
            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
                DisplayRankings();
        }
    }

    private void UpdateRankings()
    {
        GameManager.Instance.PlayerListManager.SortByScore();
        int playerCnt = GameManager.Instance.PlayerListManager.PlayerList.Count;

        for (int i = 1; i < 8; i++)
        {
            if (i <= playerCnt)
            {
                EnemyPlayer p = GameManager.Instance.PlayerListManager.PlayerList[i - 1];

                Text[] texts = playerSets[i].gameObject.GetComponentsInChildren<Text>();
                Image[] images = playerSets[i].gameObject.GetComponentsInChildren<Image>(true);
                images[1].sprite = p.Avatar.Icon;
                texts[1].text = p.Name; 
                texts[2].text = p.CurrentScore.ToString();
                playerSets[i].SetActive(true); 
            }
            else
                playerSets[i].SetActive(false);
        }   
    }

    public void DisplayImpressum()
    {
        ChangeScreen(impressumScreen);
    }

    public void DisplayRankings()
    {
        UpdateRankings();
        ChangeScreen(rankingsScreen);
    }
    
    public void ResetGame()
    {
        NotifyObservers(true);
        GameManager.Instance.ResetGame();
    }

    public override void Hide()
    {
        isShowing = false;
        base.Hide();
    }

    public override void Show()
    {
        Player player = GameManager.Instance.Player;
        base.Show();
        isShowing = true;

        playerScore.text = player.CurrentScore.ToString()+" Punkte";
        playerRank.text = "#"+GameManager.Instance.PlayerListManager.GetRankOfPlayer(player.playerId);
        DisplayRankings();
    }

    #region observer methods
    public void ObserverUpdate(int obj)
    {
        if (isShowing)
            UpdateRankings();
    }

    //for reset
    public void AddObserver(IObserver<bool> observer)
    {
        if (!observers.Contains(observer)) { observers.Add(observer); }
    }

    public void RemoveObserver(IObserver<bool> observer)
    {
        if (observers.Contains(observer)) { observers.Remove(observer); }
    }

    public void NotifyObservers(bool shouldReset)
    {
        // not using foreach, since observers can remove themselves on execution
        // and modify observers list
        for (int i = observers.Count - 1; i >= 0; i--)
        {
            observers[i].ObserverUpdate(shouldReset);
        }
    }

    #endregion
}
