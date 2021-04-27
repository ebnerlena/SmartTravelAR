using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR;
using System;

[RequireComponent(typeof(ARPlaneManager))]
public class ARObjectsManager : MonoBehaviour, IResetable, IObserver<bool>
{
    public GameObject placementIndicator;
    private GameObject objectToSpawn;
    private int packageCounter;
    private bool placementIsActive;
    private bool selectionIsActive;
    public bool isActive;
    private RaycastHit selectedHitObject;
    private int selected;

#pragma warning disable 0649
    [SerializeField]
    private InCity inCity;
    [SerializeField]
    private InCityPackageHandler packageHandler;
    [SerializeField]
    private Text uiText;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private ARSession arSession;
    private ARReferencePointManager referencePointManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool allowedToContinue = false;

    private Dictionary<int, ARReferencePoint> referencePoints;
    private Dictionary<int, GameObject> packagePrefabs;
    private Dictionary<int, GameObject> packageHistory;
    private List<SightseeingPackage> packages;

    void Start()
    {
        if (!MobileOnlyActivator.IsMobile)
           return;

        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        referencePointManager = GetComponent<ARReferencePointManager>();
        packagePrefabs = new Dictionary<int, GameObject>();
        packageHistory = new Dictionary<int, GameObject>();
        referencePoints = new Dictionary<int, ARReferencePoint>();
        placementIndicator.SetActive(false);
        HidePlaneDetection();
        isActive = false;
        placementIsActive = false;
        selectionIsActive = false;     
    }

    public void Activate()
    {
        //this.enabled = true;
        uiText.text = "Schau dich um...";
        isActive = true;
        ActivatePlacement();  
        allowedToContinue = false;
    }

    public void Deactivate()
    {

        uiText.text = "";
        HideAllObjects();
        isActive = false;
        placementIndicator.SetActive(false);
        HidePlaneDetection();
    }

    public void ActivatePlacement()
    {
        packageCounter = 1;
        selectionIsActive = false;
        placementIsActive = true;
        ShowPlaneDetection();

        packagePrefabs.Clear();

        foreach (SightseeingPackage pack in packageHandler.packages)
        {
            GameObject prefab = ResourceLoader.PackagePrefab(pack.prefabName);

            if (prefab != null)
                packagePrefabs.Add(pack.days, prefab);
        } 
        objectToSpawn = packagePrefabs[packageCounter];
    }

    void Update()
    {
        try {
            if (isActive && placementIsActive)
            {
                UpdatePlacementPose();
                UpdatePlacementIndicator();

                if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    PlaceObject();
                }
            }  
            if (isActive && selectionIsActive)
            {
                CheckForSelection();
            }

            //todo: 
            //remove - is here only for interaction chance when in citystay
            if (isActive && !placementIsActive && !selectionIsActive && allowedToContinue)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        Ray ray = Camera.current.ScreenPointToRay(touch.position);

                        if (Physics.Raycast(ray, out _))
                        {
                            ClearRestObject();
                            inCity.DisplayCityChoiceScreen();                      
                        }
                    }
                }
            }
        }
        catch(Exception e) {
             DebugText.SetText(1, "7 " + e.Message);
        }
        
    }

    public void ChooseObject(int nr)
    {
        uiText.text = "";
        selectedHitObject.transform.GetComponent<Transform>().localScale = new Vector3(2.0f, 2.0f, 2.0f);
        TextMesh textMesh = selectedHitObject.transform.GetComponentInChildren<TextMesh>();
        textMesh.text = " ";
        selected = nr;
        ClearObjectsWithout(nr);
        StartCoroutine(WaitAfterSelection());
    }

    private void ShowPlaneDetection()
    {
        planeManager.enabled = true;

        foreach(ARPlane plane in planeManager.trackables)
            plane.gameObject.SetActive(planeManager.enabled);
    }

    private void HidePlaneDetection()
    {
        planeManager.enabled = false;

        foreach (ARPlane plane in planeManager.trackables)
            plane.gameObject.SetActive(planeManager.enabled);
    }

    private void PlaceObject()
    {
        try {
            ARReferencePoint referencePoint = referencePointManager.AddReferencePoint(placementPose);
            //referencePointManager.referencePointPrefab = objectToSpawn;

            if (referencePoint != null)
            {
                referencePoints.Add(packageCounter, referencePoint);
                GameObject packageObject = Instantiate(objectToSpawn, placementPose.position, Quaternion.Euler(0, 180, 0));
                packageObject.transform.SetParent(referencePoint.transform, true);
                packageHistory.Add(packageCounter, packageObject);

                if (packageCounter < 3)
                {
                    packageCounter++;
                    objectToSpawn = packagePrefabs[packageCounter];
                }
                else
                {
                    uiText.text = "";
                    objectToSpawn = null;
                    placementIsActive = false;
                    placementIndicator.SetActive(false);
                    HidePlaneDetection();
                    StartCoroutine(WaitAfterPlacementFinished());
                }
            }
        }
        catch(Exception e)
        {
            DebugText.SetText(1, "9 " + e.Message);
        }
        
    }

    private void CheckForSelection()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.current.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                    TextMesh obj = hitObject.transform.GetComponentInChildren<TextMesh>();
                    selectedHitObject = hitObject;

                    if (obj != null)
                    {
                        inCity.ShowPackageInfoScreen((Int32.Parse(obj.text)));
                    }
                }
            }
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
            placementIndicator.SetActive(false);
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            uiText.text = "Tippe um ein Objekt zu platzieren";
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void ClearRestObject()
    {   
        referencePointManager.RemoveReferencePoint(referencePoints[selected]);
        referencePoints.Clear();

        ClearPackageHistory();
    }

    private void ClearObjectsWithout(int nr)
    {
        if (referencePoints == null)
            return;

        foreach (KeyValuePair<int, ARReferencePoint> referencePoint in referencePoints)
        {
            if (referencePoint.Key != nr)
                referencePointManager.RemoveReferencePoint(referencePoint.Value);
        }
    }

    private void HideAllObjects()
    {
        if (referencePoints == null)
            return;

        foreach (KeyValuePair<int, ARReferencePoint> referencePoint in referencePoints)
        {
            if (referencePoint.Value != null)
                 referencePointManager.RemoveReferencePoint(referencePoint.Value); 
        }

        if (referencePoints == null)
            referencePoints.Clear();

       ClearPackageHistory();
        
    }

    private void ClearPackageHistory()
    {
        foreach (KeyValuePair<int, GameObject> package in packageHistory)
        {
            if (package.Value != null)
                Destroy(package.Value);
        }
        packageHistory.Clear();
    }

    IEnumerator WaitAfterPlacementFinished()
    {
        yield return new WaitForSeconds(2);
        selectionIsActive = true;
        uiText.text = "Wähle ein Objekt aus";

    }

    IEnumerator WaitAfterSelection()
    {
        yield return new WaitForSeconds(5);
        selectionIsActive = false;
        allowedToContinue = true;
        uiText.text = "Antippen um fortzufahren";
    }

    public void Reset()
    {   
        HideAllObjects();
        HidePlaneDetection();
        //dont think that that one is working - should reset detectedplanes
        //arSession.Reset();
    }

    public void ObserverUpdate(bool shouldReset)
    {
        Reset();
    }
}