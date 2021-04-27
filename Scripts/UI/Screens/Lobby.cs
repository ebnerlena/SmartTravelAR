using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Lobby : Screen, IObserver<int>
{
#pragma warning disable 0649
    [SerializeField]
    public GameObject startScreen, inLobbyScreen, createGameScreen, chooseAvatarScreen, joinGameScreen;

    public InputField playerName, gameIdJoin, gameIdCreate;
    private AvatarType avatarType = AvatarType.Student;

    [SerializeField]
    private WeightsController weights;

    [SerializeField]
    private GenderToggle genderToggle;

    [SerializeField]
    public Toggle isPlaying;

    [SerializeField]
    private GameObject backButton, startButton, playerListContent, playerLeftPrefab, playerRightPrefab;

    [SerializeField]
    private Text lobbyIdText, joinIdText, playerCount;

    private bool inLobby = false;

#pragma warning restore 0649
    void Awake()
    {
        screens.Add(startScreen);
        screens.Add(createGameScreen);
        screens.Add(joinGameScreen);
        screens.Add(inLobbyScreen);
        screens.Add(chooseAvatarScreen);
        startButton.SetActive(false);
    }

    void Start()
    {
        GameManager.Instance.PlayerListManager.AddObserver(this);
    }

    void Update()
    {
        // Make sure user is on Android platform
        if (Application.platform == RuntimePlatform.Android && !inLobby)
        {
            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
                BackToPreviousScreen();
        }
    }

    public void StartGame()
    {
        GameManager.Instance.NetworkPlayer.SendStartGameCommand();
        playerName.text = "";
        gameIdJoin.text = "";
        gameIdCreate.text = "";
        weights.Reset();
    }

    public void JoinGame()
    {
        string username = playerName.text;
        string gameId = gameIdJoin.text;

        if (username == null || username.Length < 4)
            GameManager.Instance.SetErrorMessage(ErrorMessageType.JoinLobbyPlayernameError, "Spielername zu kurz");

        else if (gameId == null || gameId.Length < 2)
            GameManager.Instance.SetErrorMessage(ErrorMessageType.JoinLobbyIDError,"SpielID nicht gültig");

        else
        {
            GameManager.Instance.AdaptPlayer(username, avatarType);
            GameManager.Instance.NetworkPlayer.SendJoinLobbyCommand(gameId, GameManager.Instance.Player.GetInfo());

            // if player is offline and wants to join test lobby
            if (GameManager.Instance.NetworkPlayer.Status.Equals(NetworkStatus.Offline) && gameId != null && gameId.Equals("test"))
            {
                GameManager.Instance.NetworkPlayer.SendAndHandle(JoinedLobbySuccessMessageable.JoinTestLobby);
                StartGame();
            }
        }
    }

    public void TryConnect()
    {
        GameManager.Instance.NetworkPlayer.TryConnect();
    }
    public void Disconnect()
    {
        GameManager.Instance.NetworkPlayer.Disconnect();
    }

    public void CreateGame()
    {
        string gameId = gameIdCreate.text;

        if (gameId != null && gameId.Length > 1)
        {
            weights.GetWeights(out var resWeights, out float daysWeight, out float cultureWeight);
            GameManager.Instance.NetworkPlayer.SendCreateLobbyCommand(gameId, daysWeight, cultureWeight, resWeights);
        }
        else
            GameManager.Instance.SetErrorMessage(ErrorMessageType.JoinLobbyIDError, "SpielID nicht gültig");
    }

    public void OnCreateLobbySuccess()
    {
        if (isPlaying.isOn)
        {
            DisplayAvatarChoiceScreen();
            screenHistory.Add(createGameScreen);
        }
        else
        {
            GameManager.Instance.Player.isViewer = true;
            GameManager.Instance.NetworkPlayer.SendJoinLobbyAsViewerCommand(gameIdCreate.text);
            
            DisplayInLobbyScreen();
        }

        GameManager.Instance.ExecuteOnMain(() => { lobbyIdText.text = gameIdCreate.text; joinIdText.text = gameIdCreate.text; });
        startButton.SetActive(true);
    }

    public void AvatarChoice(string type)
    {
        avatarType = (AvatarType)Enum.Parse(typeof(AvatarType),type);
        DisplayPlayerSettings();
    }

    public void DisplayGameSettings()
    {
        ChangeScreen(createGameScreen);
        screenHistory.Add(startScreen);
    }

    public void DisplayPlayerSettings()
    {
        ChangeScreen(joinGameScreen);
        screenHistory.Add(chooseAvatarScreen);
        playerName.text = "";
        gameIdJoin.text = "";
    }

    public void DisplayInLobbyScreen()
    {
        ChangeScreen(inLobbyScreen);
        screenHistory.Add(joinGameScreen);
        lobbyIdText.text = gameIdJoin.text;
        inLobby = true;
    }

    public void DisplayAvatarChoiceScreen()
    {
        ChangeScreen(chooseAvatarScreen);
        screenHistory.Add(startScreen);
    }

    public void UpdatePlayerList()
    {
        var children = new List<GameObject>();
        foreach (Transform child in playerListContent.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        int playerListOffset = 0;
        List<EnemyPlayer> players = GameManager.Instance.PlayerListManager.PlayerList;

        for (int i = 0; i < players.Count; i++)
        {
            GameObject prefab = null;
            if (i % 2 == 0)
                prefab = playerLeftPrefab;
            else
                prefab = playerRightPrefab;
             
                  
            GameObject player = Instantiate(prefab, playerListContent.transform);        
            var t = player.GetComponent<RectTransform>();
            t.anchoredPosition = new Vector2(0.0f, playerListOffset);
            player.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            Text[] textField = player.GetComponentsInChildren<Text>();
            textField[0].text = players[i].Name;
            player.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = ResourceLoader.Avatar(players[i].Avatar);
            playerListOffset -= 250;
        }
    }

    protected override void ChangeScreen(GameObject newScreen)
    {
        base.ChangeScreen(newScreen);

        if (newScreen == startScreen || newScreen == inLobbyScreen)
            backButton.SetActive(false);
        else
            backButton.SetActive(true);
    }

    private void DisplayStart()
    {
        ChangeScreen(startScreen);
    }

    public void BackToPreviousScreen()
    {
        ChangeScreen(screenHistory[screenHistory.Count - 1]);
        screenHistory.RemoveAt(screenHistory.Count - 1);
    }

    public override void Show()
    {
        base.Show();
        DisplayStart();
        screenHistory.Clear();
        inLobby = false;
    }

    public void ObserverUpdate(int obj)
    {
        GameManager.Instance.ExecuteOnMain(() => {
            playerCount.text = obj.ToString();
            UpdatePlayerList();
        });
    }

    void OnEnable()
    {
        GameManager.Instance?.PlayerListManager?.AddObserver(this);
    }

    void OnDisable()
    {
        GameManager.Instance.PlayerListManager.RemoveObserver(this);
    }
}
