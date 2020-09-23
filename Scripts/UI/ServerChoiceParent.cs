using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerChoiceParent : MonoBehaviour
{
    public int defaultIndex;
    public List<NetworkStatus> statusOrder;
    public Color colorActive, colorInactive;
    public List<Button> choiceButtons;

    private int selectedIndex;

    void Awake()
    {
        for(int i = 0; i < choiceButtons.Count; i++)
        {
            int idx = i;
            choiceButtons[i].onClick.AddListener(() => OnClicked(idx));
        }

        OnClicked(defaultIndex);
    }

    private void OnClicked(int index)
    {
        selectedIndex = index;
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].image.color = index == i ? colorActive : colorInactive;
        }
    }

    public NetworkStatus GetSelectedStatus()
    {
        if (selectedIndex < statusOrder.Count)
            return statusOrder[selectedIndex];
        else
            return NetworkStatus.Online;
    }
}
