using UnityEngine;
using System.Collections.Generic;


public abstract class Screen : MonoBehaviour
{
    public GameObject screen;

    protected List<GameObject> screenHistory = new List<GameObject>();
    protected  List<GameObject> screens = new List<GameObject>();

    void Awake()
    {
        this.screen = this.gameObject;
    }

    public virtual void Show()
    {
        screen.SetActive(true);
    }

    public virtual void Hide()
    {
        screen.SetActive(false);
    }

    protected virtual void ChangeScreen(GameObject newScreen)
    {
        foreach (GameObject screen in screens)
        {
            if (screen != newScreen)
                screen.SetActive(false);
        }
        newScreen.SetActive(true);
        GameManager.Instance?.SetErrorMessage(ErrorMessageType.ClearError,"");
    }
}
