using System;
using System.Collections.Generic;

public class NetworkPlayer : IObserver<NetworkMessage>
{
    private string localPlayerId;
    private string lobbyId;
    public NetworkStatus Status { get; private set; }

    private Client client;
    private ServerChoiceParent serverChoice;

    public NetworkPlayer(string localPlayerId, ServerChoiceParent serverChoice)
    {
        this.localPlayerId = localPlayerId;
        Status = NetworkStatus.Offline;
        this.serverChoice = serverChoice;
    }

    public void TryConnect()
    {
        if (client == null)
            client = new Client();

        NetworkStatus selectedStatus = serverChoice.GetSelectedStatus();
        if (client.ConnectByNetworkStatus(selectedStatus))
        {
            client.AddObserver(this);
            Status = selectedStatus;
            GameManager.Instance.SetErrorMessage(ErrorMessageType.ClearServerError, "");
            UnityEngine.Debug.Log("Connect to the Server! :) (type: "+selectedStatus+")");
        }
        else
        {
            GameManager.Instance.SetErrorMessage(ErrorMessageType.ServerError, "keine Server-Verbindung :(");
            UnityEngine.Debug.Log("Couldn't connect to the Server! :(");
        }
    }

    public void Disconnect()
    {
        if (client != null && IsOnline())
            client.Disconnect();
    }

    public void SendOnly(NetworkMessage message)
    {
        if(IsOnline())
        {
            //message.playerId = localPlayerId;
            //message.lobbyId = lobbyId;
            client.Share(message);
        }
    }

    public void SendAndHandle(NetworkMessage message)
    {
        // sharing before handling
        // so that if handle generates more commands, correct order is kept
        SendOnly(message);
        NetworkMessageHandler.Handle(message);
    }

    public void Receive(NetworkMessage message)
    {
        NetworkMessageHandler.Handle(message);
    }

    public void SetNextBufferSize(int size)
    {
        client.SetNextBufferSize(size);
    }

    public void SendCreateLobbyCommand(string lobbyId, float daysWeight, float cultureWeight, Dictionary<Type, float> lobbyWeights)
    {
        DictionaryStripper.ExtractWeightDic(lobbyWeights, out string[] resNames, out float[] weights);
        SendAndHandle(new CreateLobbyMessageable(lobbyId, daysWeight, cultureWeight, resNames, weights));
    }

    // called when executing join lobby command
    public void ActuallyJoinLobby(string lobbyId)
    {
        this.lobbyId = lobbyId;
        //GameManager.Instance.Status = GameStatus.InCity;
        UnityEngine.Debug.Log("Joined lobby " + lobbyId);
    }

    // called by UI
    public void SendJoinLobbyCommand(string lobbyid, PlayerInfo playerInfo)
    {
        SendOnly(new JoinLobbyMessageable(lobbyid, playerInfo));
    }

    // called by UI
    public void SendStartGameCommand()
    {
        SendAndHandle(new StartGameMessageable());
    }

    public void SendJoinLobbyAsViewerCommand(string lobbyId)
    {
        SendOnly(new JoinLobbyAsViewerMessageable(lobbyId));
    }

    public void Ping()
    {
        SendOnly(new PingCommand());
        //SendTestCommands();
    }

    private void SendTestCommands()
    {
        //CityUpdateMessageable cityMsg = new CityUpdateMessageable("wien4life", "all the time", new string[] { "Time", "Money" }, new float[] { 200, 100 });
        //SendOnly(cityMsg);
        //TripUpdateMessegable tripMsg = new TripUpdateMessegable(100, DateTime.Now, "Wien", "Barcelona", "Auto");
        //SendOnly(tripMsg);
        //PlayerJoinedMessageable playerMsg = new PlayerJoinedMessageable("lorenz der gott", "Student");
        //SendOnly(playerMsg);
    }

    public void ObserverUpdate(NetworkMessage message)
    {
        Receive(message);
    }

    private bool IsOnline()
    {
        return Status.Equals(NetworkStatus.Online) || Status.Equals(NetworkStatus.LocalNetwork);
    }

    public void SendOnly(INetworkMessageable message)
    {
        SendOnly(message.ToNetworkMessage());
    }

    public void SendAndHandle(INetworkMessageable message)
    {
        SendAndHandle(message.ToNetworkMessage());
    }
}