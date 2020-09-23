using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Viewer : Screen, IObserver<int>
{
    
    public Text lobbyId;
    public List<ViewerPlayerDisplay> playerDisplays;

    public Text studentTimeText, businessTimeText;
    public string studentTimePrefix, businessTimePrefix, studentTimePostfix, businessTimePostfix;
    public float timeUpdateRate = 2f;

    private float studentTime, businessTime;
    private float t;

    void Awake()
    {
        studentTime = AvatarFactory.CreateTypeOf(AvatarType.Student).MinutesToPlay;
        businessTime = AvatarFactory.CreateTypeOf(AvatarType.Businessman).MinutesToPlay;
    }

    void Start()
    {
        GameManager.Instance.PlayerListManager.AddObserver(this);
    }

    void Update()
    {
        t += Time.deltaTime;
        if(t >= timeUpdateRate)
        {
            t = 0f;
            if (studentTimeText != null)
                studentTimeText.text = studentTimePrefix + TimeManager.Instance.GetRemainingDays(studentTime).ToString("00.#") + studentTimePostfix;
            if (businessTimeText != null)
                businessTimeText.text = businessTimePrefix + TimeManager.Instance.GetRemainingDays(businessTime).ToString("00.#") + businessTimePostfix;
        }
    }

    private void UpdateTop3()
    {
        GameManager.Instance.PlayerListManager.SortByScore();
        var players = GameManager.Instance.PlayerListManager.GetTop3();

        for (int i = 0; i < players.Count && i < playerDisplays.Count; i++)
        {
            playerDisplays[i].AdaptTo(players[i]);
        }
    }

    public override void Show()
    {
        base.Show();
        UpdateTop3();
    }

    public void ObserverUpdate(int obj)
    {
        GameManager.Instance.ExecuteOnMain(UpdateTop3);
    }
}
