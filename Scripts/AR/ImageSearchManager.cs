/*
using GoogleARCore;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageSearchManager : MonoBehaviour
{
    public static ImageSearchManager Instance { get; private set; }

    private InCity incity;
    private Travelling travelling;
    private Player localPlayer;

    private string curSearchName;

    public void OnImageFound()
    {
        //travelling.ArrivedInCity();
    }

    public void OnAnchorFound(Anchor anchor)
    {
        SpawnCityModel(anchor);
    }

    private void SpawnCityModel(Anchor anchor)
    {
        GameObject cityModel = GameManager.Instance.GetCityModel(curSearchName);
        if (cityModel != null)
        {
            GameObject modelInstance = Instantiate(cityModel, anchor.transform);
            modelInstance.transform.localPosition = Vector3.zero;
            modelInstance.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public string SpawnModel(GameObject model,Anchor anchor)
    {
        string x = "before";
        GameObject modelInstance = Instantiate(model, anchor.transform);
        x = "after";
        //modelInstance.transform.localPosition = Vector3.zero;
        //modelInstance.transform.localRotation = Quaternion.Euler(0, 0, 0);

        return x;
        
    }

    public void Setup(InCity incity, Travelling travelling, Player localPlayer)
    {
        this.incity = incity;
        this.travelling = travelling;
        this.localPlayer = localPlayer;
    }
}
*/
