using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using System.Collections;

public class MobileOnlyActivator : MonoBehaviour
{
    public static bool IsMobile { get; private set; }
    public Button scanMarkerButton;

#pragma warning disable 0649
    [SerializeField]
    private ARSession m_Session;

    void Awake()
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        gameObject.SetActive(true);
        IsMobile = true;
#else
        gameObject.SetActive(false);
        IsMobile = false;
#endif

        if (scanMarkerButton != null)
            scanMarkerButton.gameObject.SetActive(!IsMobile);
        if (IsMobile)
            StartCoroutine(CheckSupport());
    }

    IEnumerator CheckSupport()
    {
        yield return ARSession.CheckAvailability();

        if (ARSession.state == ARSessionState.NeedsInstall)
        {
            yield return ARSession.Install();
        }

        if (ARSession.state == ARSessionState.Ready)
        {
            m_Session.enabled = true;
        }
        else
        {
            switch (ARSession.state)
            {
                case ARSessionState.Unsupported:
                    Debug.Log("Your device does not support AR.");
                    GameManager.Instance.SetErrorMessage(ErrorMessageType.ARError, "AR wird leider nicht unterstützt");
                    CameraController.Instance.ShowDefaultCamera();
                    IsMobile = false;
                    break;
                case ARSessionState.NeedsInstall:
                    Debug.Log("The software update failed, or you declined the update.");
                    GameManager.Instance.SetErrorMessage(ErrorMessageType.ARError, "AR Software Update muss installiert werden");
                    CameraController.Instance.ShowDefaultCamera();
                    break;
            }
        }
    }

    /*
    IEnumerator Start()
    {
        if ((ARSession.state == ARSessionState.None) ||
            (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported)
        {
            GameManager.Instance.SetErrorMessage(ErrorMessageType.ARError, "AR nicht verfügbar");
            scanMarkerButton.gameObject.SetActive(true);
            CameraController.Instance.ShowDefaultCamera();

            if (IsMobile)
                IsMobile = false;

        }
        else
            scanMarkerButton.gameObject.SetActive(false);
    } */
}
