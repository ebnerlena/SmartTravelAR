using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MinigameController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private GameObject ballBalancerScreen;

    [SerializeField]
    private GameObject shakerScreen;

    public static MinigameController Instance { get; private set; }
    private Minigame currentMinigame;
    private List<GameObject> minigameScreens = new List<GameObject>();
    private List<Minigame> minigames = new List<Minigame>();
    private GameObject currentMinigameScreen;

    private System.Random rnd;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);

        minigameScreens.Add(ballBalancerScreen);
        minigameScreens.Add(shakerScreen);
        rnd = new System.Random();
    }

   void Start()
    {
        //getting all children minigames in list 
        GetComponentsInChildren<Minigame>(true, minigames);
        currentMinigame = minigames[0];
        currentMinigameScreen = minigameScreens[0];

    }
    public void LoadGame()
    {
        int nextGame = rnd.Next(0, minigames.Count);
        currentMinigameScreen = minigameScreens[nextGame];
        currentMinigameScreen.SetActive(true);
        currentMinigame = minigames[nextGame];
        currentMinigame.StartGame(); 
    }

    public void StopCurrentGame()
    {
        currentMinigame.StopGame();
        currentMinigameScreen.SetActive(false);

        if (MobileOnlyActivator.IsMobile)
           CameraController.Instance.ShowFirstPersonView();
        else
            CameraController.Instance.ShowDefaultCamera();
    }

    public bool GetSuccess()
    {
        return currentMinigame.GetSuccess();
    }
}
