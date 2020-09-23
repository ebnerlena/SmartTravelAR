using System;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeDisplay : MonoBehaviour
{
    public Image circle;
    public Text text;
    public string textFormat = "noch {0} Tage";
    public float updateSeconds = 0.5f;

    private DateTime gameStart;
    private float minsToPlay;
    private bool initiated = false;

    private bool hookedToEvent;
    private float t = 0f;

    void OnEnable()
    {
        GameTimeDisplayParent parent = GameTimeDisplayParent.Instance;
        if (parent == null) return;

        if (parent.IsInitiated)
        {
            var data = parent.GetInitiationData();
            Initiate(data.Item1, data.Item2);
        }
        else
        {
            parent.OnInitiate += Initiate;
            hookedToEvent = true;
        }
    }

    public void Initiate(DateTime gameStart, float minsToPlay)
    {
        this.gameStart = gameStart;
        this.minsToPlay = minsToPlay;
        initiated = true;
    }

    void Update()
    {
        if (!initiated)
            return;
        t += Time.deltaTime;

        if (t > updateSeconds)
        {
            t = 0f;
            double remainMins = TimeManager.Instance.GetRemainingGameTimeMins();
            double remainDays = TimeManager.Instance.GetRemainingDays();


            if (circle != null)
                circle.fillAmount = (float)(remainMins / minsToPlay);
            if (text != null)
                text.text = string.Format(textFormat, remainDays.ToString("#00"));
        }
    }

    public void InitiateNow(float minsToPlay)
    {
        Initiate(DateTime.UtcNow, minsToPlay);
    }

    void OnDisable()
    {
        if (hookedToEvent)
            GameTimeDisplayParent.Instance.OnInitiate -= Initiate;
    }
}
