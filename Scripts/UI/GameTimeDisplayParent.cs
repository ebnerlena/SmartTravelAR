using System;
using UnityEngine;

public class GameTimeDisplayParent : MonoBehaviour
{
    public static GameTimeDisplayParent Instance { get; private set; }
    public bool IsInitiated { get; private set; }

    public delegate void InitiationMethod(DateTime gameStart, float minsToPlay);
    public event InitiationMethod OnInitiate;

    private float minsToPlay;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        IsInitiated = false;
    }

    public void Initiate(float minsToPlay)
    {
        this.minsToPlay = minsToPlay;
        IsInitiated = true;

        OnInitiate?.Invoke(TimeManager.Instance.GameStart, minsToPlay);
    }

    public (DateTime,float) GetInitiationData()
    {
        return (TimeManager.Instance.GameStart, minsToPlay);
    }
}
