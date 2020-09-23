using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour, IObserver<int>
{
    public static Map Instance { get; private set; }

    public RectTransform mask;
    public RectTransform map, viewerMap;
    public string[] cityNames;
    public Transform[] cities;
    public Transform defaultMarkerPosition;
    public Transform maxTopLeft, maxBotRight;

    private Transform localPlayerMarker;
    public GameObject playerMarkerPrefab;
    public Dictionary<City, Vector2> cityPositions = new Dictionary<City, Vector2>();
    public Dictionary<string, MapPlayerMarker> playerMarkers = new Dictionary<string, MapPlayerMarker>();

    private bool isBig;
    private Vector3 smallMapPos;
    
    private (Vector2, Vector2) maskAnchors;
    private Vector2 maskSizeDelta;
    private Vector2 maskAnchoredPos;
    public Vector2 maskBigSizeDelta;
    public float switchSpeed;

    private Image mapImage;
    private float mapSmallAlpha;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        isBig = false;
        
        playerMarkers = new Dictionary<string, MapPlayerMarker>();
        cityPositions = new Dictionary<City, Vector2>();
        smallMapPos = new Vector3();

        maskAnchors = (mask.anchorMin, mask.anchorMax);
        maskSizeDelta = mask.sizeDelta;
        maskAnchoredPos = mask.anchoredPosition;
        switchSpeed = Mathf.Max(1, switchSpeed);
    }

    void Start()
    {
        FillDictionaries();

        mapImage = map.GetComponent<Image>();
        if (mapImage != null)
            mapSmallAlpha = mapImage.color.a;
        else
            mapSmallAlpha = 1;
    }

    public void MovePlayer(string playerId, string cityName)
    {
        Debug.Log("moving player "+playerId+" to "+cityName);
        City foundCity = GraphGenerator.GetCity(cityName);
        if(foundCity != null && cityPositions.ContainsKey(foundCity))
        {
            MovePlayer(playerId, cityPositions[foundCity]);
        }
    }

    public void MovePlayer(string playerId, Vector2 pos)
    {
        if(playerMarkers.ContainsKey(playerId))
        {
            playerMarkers[playerId].transform.localPosition = pos;
        }
    }

    void LateUpdate()
    {
        if(localPlayerMarker != null && localPlayerMarker.localPosition != smallMapPos)
        {
            MoveMap(localPlayerMarker.localPosition);
        }
    }

    private void MoveMap(Vector2 pos)
    {
        smallMapPos = pos * -1;
        smallMapPos.Set(
            Mathf.Clamp(smallMapPos.x, maxTopLeft.localPosition.x, maxBotRight.localPosition.x),
            Mathf.Clamp(smallMapPos.y, maxBotRight.localPosition.y, maxTopLeft.localPosition.y),
            smallMapPos.z
        );

        if (map != null && !isBig)
            map.localPosition = smallMapPos;
    }

    public void MoveBetweenCities(string playerId, string cityFrom, string cityTo, float inSeconds, string transportTypeName)
    {
        City from = GraphGenerator.GetCity(cityFrom);
        City to = GraphGenerator.GetCity(cityTo);

        bool citiesExist = from != null && to != null;
        bool citiesOnMap = cityPositions.ContainsKey(from) && cityPositions.ContainsKey(to);
        bool playerSpawned = playerMarkers.ContainsKey(playerId);
        bool timeIsFine = inSeconds >= 0;
        
        if (citiesExist && citiesOnMap && playerSpawned && timeIsFine)
        {
            Sprite tSprite = ResourceLoader.TransportType(transportTypeName);
            playerMarkers[playerId].StartMoving(cityPositions[from], cityPositions[to], inSeconds, tSprite);
        }
    }

    private void FillDictionaries()
    {
        for(int i = 0; i < cityNames.Length && i < cities.Length; i++)
        {
            City foundCity = GraphGenerator.GetCity(cityNames[i]);
            if(foundCity != null)
            {
                cityPositions.Add(foundCity, cities[i].localPosition);
            }
        }
    }

    public void ToggleMap()
    {
        isBig = !isBig;
        if (isBig)
            GameManager.Instance.ExecuteOnMain(() => StartCoroutine(SwitchToBigMap()));
        else
            GameManager.Instance.ExecuteOnMain(() => StartCoroutine(SwitchToSmallMap()));
    }

    public void ObserverUpdate(int obj)
    {
        // spawning has to be on main, since adding is bound to spawning
        // everything has to be executed on main thread
        GameManager.Instance.ExecuteOnMain(OnPlayerCountChanged);
    }

    private void OnPlayerCountChanged()
    {
        foreach (EnemyPlayer player in GameManager.Instance.PlayerListManager.PlayerList)
        {
            if (!playerMarkers.ContainsKey(player.playerId))
            {
                // starting in salzburg
                Transform markerParent = GameManager.Instance.Player.isViewer ? viewerMap : map;
                MapPlayerMarker newMarker = Instantiate(playerMarkerPrefab, markerParent).GetComponent<MapPlayerMarker>();
                newMarker.Setup(player.Name, player.Avatar);
                newMarker.transform.localPosition = defaultMarkerPosition.localPosition;
                playerMarkers.Add(player.playerId, newMarker);

                if (player.playerId.Equals(GameManager.Instance.Player?.playerId))
                    localPlayerMarker = newMarker.transform;
            }
        }
    }
    private IEnumerator SwitchToSmallMap()
    {
        float progress = 0f;
        Vector2 anchorMinProg = new Vector2(0.5f, 0.5f);
        Vector2 anchorMaxProg = new Vector2(0.5f, 0.5f);
        Vector2 anchoredPosProg = Vector2.zero;
        Vector2 sizeProg = maskBigSizeDelta;
        Vector2 mapPosProg = Vector2.zero;

        // dont know, not working
        mapImage.CrossFadeAlpha(mapSmallAlpha, 1f / switchSpeed, false);

        while (progress <= 1f)
        {
            anchorMinProg.x = Mathf.Lerp(0.5f, maskAnchors.Item1.x, progress);
            anchorMinProg.y = Mathf.Lerp(0.5f, maskAnchors.Item1.y, progress);
            anchorMaxProg.x = Mathf.Lerp(0.5f, maskAnchors.Item2.x, progress);
            anchorMaxProg.y = Mathf.Lerp(0.5f, maskAnchors.Item2.y, progress);

            anchoredPosProg.x = Mathf.Lerp(0, maskAnchoredPos.x, progress);
            anchoredPosProg.y = Mathf.Lerp(0, maskAnchoredPos.y, progress);

            sizeProg.x = Mathf.Lerp(maskBigSizeDelta.x, maskSizeDelta.x, progress);
            sizeProg.y = Mathf.Lerp(maskBigSizeDelta.y, maskSizeDelta.y, progress);

            mapPosProg.x = Mathf.Lerp(0, smallMapPos.x, progress);
            mapPosProg.y = Mathf.Lerp(0, smallMapPos.y, progress);

            mask.anchorMin = anchorMinProg;
            mask.anchorMax = anchorMaxProg;
            mask.anchoredPosition = anchoredPosProg;
            mask.sizeDelta = sizeProg;

            map.localPosition = mapPosProg;

            yield return new WaitForFixedUpdate();
            progress += Time.fixedDeltaTime * switchSpeed;
        }

        mask.anchorMin = maskAnchors.Item1;
        mask.anchorMax = maskAnchors.Item2;
        mask.anchoredPosition = maskAnchoredPos;
        mask.sizeDelta = maskSizeDelta;
        map.localPosition = smallMapPos;
    }

    private IEnumerator SwitchToBigMap()
    {
        float progress = 0f;
        Vector2 anchorMinProg = maskAnchors.Item1;
        Vector2 anchorMaxProg = maskAnchors.Item2;
        Vector2 anchoredPosProg = maskAnchoredPos;
        Vector2 sizeProg = maskSizeDelta;
        Vector2 mapPosProg = smallMapPos;

        // dont know, not working
        mapImage.CrossFadeAlpha(1f, 1f / switchSpeed, false);

        while (progress <= 1f)
        {
            anchorMinProg.x = Mathf.Lerp(maskAnchors.Item1.x, 0.5f, progress);
            anchorMinProg.y = Mathf.Lerp(maskAnchors.Item1.y, 0.5f, progress);
            anchorMaxProg.x = Mathf.Lerp(maskAnchors.Item2.x, 0.5f, progress);
            anchorMaxProg.y = Mathf.Lerp(maskAnchors.Item2.y, 0.5f, progress);

            anchoredPosProg.x = Mathf.Lerp(maskAnchoredPos.x, 0, progress);
            anchoredPosProg.y = Mathf.Lerp(maskAnchoredPos.y, 0, progress);

            sizeProg.x = Mathf.Lerp(maskSizeDelta.x, maskBigSizeDelta.x, progress);
            sizeProg.y = Mathf.Lerp(maskSizeDelta.y, maskBigSizeDelta.y, progress);

            mapPosProg.x = Mathf.Lerp(smallMapPos.x, 0, progress);
            mapPosProg.y = Mathf.Lerp(smallMapPos.y, 0, progress);

            mask.anchorMin = anchorMinProg;
            mask.anchorMax = anchorMaxProg;
            mask.anchoredPosition = anchoredPosProg;
            mask.sizeDelta = sizeProg;

            map.localPosition = mapPosProg;

            yield return new WaitForFixedUpdate();
            progress += Time.fixedDeltaTime * switchSpeed;
        }

        mask.anchorMin = new Vector2(0.5f, 0.5f);
        mask.anchorMax = new Vector2(0.5f, 0.5f);
        mask.anchoredPosition = Vector2.zero;
        mask.sizeDelta = maskBigSizeDelta;
        map.localPosition = Vector2.zero;
    }
}
