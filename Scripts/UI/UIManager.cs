using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class UIManager : MonoBehaviour, IObserver<GameStatus>
{
    // to supress "never used"-warning
#pragma warning disable 0649
    private NetworkPlayer nwPlayer;
    private Player localPlayer;
    private bool permanentError;

    private List<Screen> uiStates = new List<Screen>();
    public Text errorMsg;

    [SerializeField]
    private Lobby lobby;

    [SerializeField]
    private InCity inCity;

    [SerializeField]
    private Ranking ranking;

    [SerializeField]
    private Viewer viewer;

    [SerializeField]
    private Travelling travelling;
#pragma warning restore 0649
    void Awake()
    {
        uiStates.Add(lobby);
        uiStates.Add(inCity);
        uiStates.Add(ranking);
        uiStates.Add(viewer);
        uiStates.Add(travelling);
    }

    void Start()
    {
        GameManager.Instance.AddObserver(this);
        GameManager.Instance.Status = GameStatus.Lobby;
    }

    public void AdaptToStatus(GameStatus status)
    {
        foreach(Screen screen in uiStates)
        {
            if (screen.GetType().Name != status.ToString())
                screen.Hide();
            else
                screen.Show();
        }
    }

    public void SetNWPlayer(NetworkPlayer nwPlayer)
    {
        this.nwPlayer = nwPlayer;
    }

    public void SetPlayer(Player localPlayer)
    {
        this.localPlayer = localPlayer;
    }

    public void CreatedLobbySuccess()
    {
        GameManager.Instance.ExecuteOnMain(() => lobby.OnCreateLobbySuccess());
    }

    public void ShowInLobbyScreen()
    {
        GameManager.Instance.ExecuteOnMain(lobby.DisplayInLobbyScreen);
    }

    public void SetViewerLobbyId()
    {
        viewer.lobbyId.text = lobby.gameIdCreate.text;
    }

    public void SetErrorMessage(ErrorMessageType msgType, string msg)
    {
        if (msgType == ErrorMessageType.ServerError || msgType == ErrorMessageType.ARError)
        {
            permanentError = true;
            GameManager.Instance.ExecuteOnMain(() => DisplayErrorMessage(msg));
        }
        else
        {
            if (msgType == ErrorMessageType.ClearServerError)
                permanentError = false;

            if (!permanentError)
            {
                if ((int)msgType < 5)
                    GameManager.Instance.ExecuteOnMain(() => DisplayErrorMessage(msg));
                else
                    GameManager.Instance.ExecuteOnMain(() => StartCoroutine(ToastErrorMessage(msg)));
            }  
        }  
    }

    IEnumerator ToastErrorMessage(string msg)
    {
        errorMsg.text = msg; 
        yield return new WaitForSeconds(5);

        SetErrorMessage(ErrorMessageType.ClearError,"");
    }

    private void DisplayErrorMessage(string msg)
    {
        errorMsg.text = msg; 
    }

    public void ObserverUpdate(GameStatus obj)
    {
        GameManager.Instance.ExecuteOnMain(() => AdaptToStatus(obj));
    }

    public Travelling GetTravellingScreen()
    {
        return travelling;
    }
    public InCity GetInCityScreen()
    {
        return inCity;
    }
}
