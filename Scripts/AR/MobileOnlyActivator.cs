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
    }

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
    }
}
