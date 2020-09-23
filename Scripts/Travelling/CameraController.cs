using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Camera firstPersonCamera;

    [SerializeField]
    private Camera trainCamera;

    [SerializeField]
    private Camera planeCamera;

    [SerializeField]
    private Camera carCamera;

    [SerializeField]
    private Camera defaultCamera;

    private Camera activCam;

    private List<Camera> cameras = new List<Camera>();

    public static CameraController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);

        cameras.Add(firstPersonCamera);
        cameras.Add(planeCamera);
        cameras.Add(carCamera);
        cameras.Add(trainCamera);
        cameras.Add(defaultCamera);
    }

    private void ChooseMinigameCamera(TransportType type)
    {

        switch (TypeHelper.ToString(type.GetType()))
        {
            case "TrainTransport":
                activCam = trainCamera;
                break;

            case "PlaneTransport":
                activCam = planeCamera;
                break;

            case "CarTransport":
                activCam = carCamera;
                break;
        }
    }

    public void ShowMinigameCamera()
    {
        ChooseMinigameCamera(GameManager.Instance.Player.Trip.CurrentTransport.Option.TransportType);
        SwitchCamera();
    }

    public void ShowFirstPersonView()
    {
        activCam = firstPersonCamera;
        SwitchCamera();
    }

    public void ShowDefaultCamera()
    {
        activCam = defaultCamera;
        SwitchCamera();
    }

    private void SwitchCamera()
    {
        foreach (Camera cam in cameras)
        {
            if (cam != activCam)
                cam.enabled = false;

            else
                cam.enabled = true;
        }
    }
}
