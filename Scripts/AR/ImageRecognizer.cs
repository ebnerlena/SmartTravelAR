using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class ImageRecognizer : MonoBehaviour
{
    public GameObject ScanCue;
    private ARTrackedImageManager trackedImageManager;
    private ARRaycastManager raycastManager;
    private Vector2 touchPosition;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public delegate void OnImageFound();
    private OnImageFound onImageFound;

    private bool searchForImage;
    private string searchImageName;
    private GameObject currentPrefab;
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();


#pragma warning disable 0649
    [SerializeField]
    private GameObject[] transportPrefabs;
    [SerializeField]
    private Text uiText;
    [SerializeField]
    private Travelling travelling;


    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        raycastManager = GetComponent<ARRaycastManager>();

        foreach (GameObject prefab in transportPrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.SetActive(false);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }
        this.enabled = false;
    }

    public void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
        ScanCue.SetActive(true);
        uiText.text = "";
        searchImageName = "";
    }

    public void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
        searchForImage = false;
        ScanCue.SetActive(false);
        searchImageName = "";
        uiText.text = "";
        HideAllObjects();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0 
            && !EventSystem.current.IsPointerOverGameObject(
                        Input.GetTouch(0).fingerId))
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        else
        {
            touchPosition = default;
            return false;
        }
    }

    public void SearchForImage(string imageName, OnImageFound onImageFound)
    {
        HideAllObjects();
        uiText.text = "";
        searchForImage = true;
        this.searchImageName = imageName;
        this.onImageFound = onImageFound;
        ScanCue.SetActive(true);
    }


    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        Touch touch = Input.GetTouch(0);
        touchPosition = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.current.ScreenPointToRay(touch.position);
            RaycastHit hitObject;

            if (Physics.Raycast(ray, out hitObject))
            {
                currentPrefab.SetActive(false);
                travelling.CityCheckIn();
            }
        }
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (searchForImage)
        {
            foreach (ARTrackedImage trackedImage in args.added)
                UpdateImage(trackedImage);

            foreach (ARTrackedImage trackedImage in args.updated)
                UpdateImage(trackedImage);
        }
    }

    void UpdateImage(ARTrackedImage trackedImage)
    {
        string imageCityName = trackedImage.referenceImage.name;

        if (searchImageName == imageCityName && trackedImage.trackingState == TrackingState.Tracking)
        {
            string transportType = GameManager.Instance.GetTransportType();
            Vector3 pos = trackedImage.transform.position;

            currentPrefab = spawnedPrefabs[transportType];
            uiText.text = "Antippen um Einzuchecken";

            currentPrefab.transform.position = pos;
            currentPrefab.SetActive(true);
            ScanCue.SetActive(false);
            onImageFound?.Invoke();

            HideAllObjectsExcept(transportType);
            searchImageName = "";
        }
    }

    private void HideAllObjectsExcept(string name)
    {
        foreach (GameObject go in spawnedPrefabs.Values)
        {
            if (go.name != currentPrefab.name)
                go.SetActive(false);
        }
    }

    private void HideAllObjects()
    {
        foreach (GameObject go in spawnedPrefabs.Values)
            go.SetActive(false);
    }
}
