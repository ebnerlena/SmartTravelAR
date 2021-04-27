using UnityEngine;
using UnityEngine.UI;

public class Minigame : MonoBehaviour
{
    public Text message;

    protected bool success;

    void Awake()
    {
        success = true;
    }

    public virtual void StartGame()
    {
        CameraController.Instance.ShowMinigameCamera();
        this.gameObject.SetActive(true);
        UnityEngine.Screen.sleepTimeout = SleepTimeout.NeverSleep;
        success = true;
    }

    public virtual void StopGame()
    {
        message.text = "";
        this.gameObject.SetActive(false);
        CameraController.Instance.ShowFirstPersonView();
    }

    public bool GetSuccess()
    {
        return success;
    }
}