public class NetworkMessageHandler
{
    public delegate INetworkCommand MessageToCommandFunction(NetworkMessage message);

    public static void Handle(NetworkMessage message)
    {
        //UnityEngine.Debug.Log("handling "+ message.typeName);
        INetworkCommand cmd = ConvertToCommand(message);
        QueueCommand(cmd, message.typeName);
    }

    private static INetworkCommand ConvertToCommand(NetworkMessage message)
    {
        MessageToCommandFunction converterFunc = null;

        switch (message.typeName)
        {
            // have to use explicit names and functions
            // because static members cannot be inherited or marked as abstract
            case PingMessageable.MSGTYPE:
                converterFunc = PingCommand.FromNetworkMessage;
                break;
            case JoinedLobbySuccessMessageable.MSGTYPE:
                converterFunc = JoinedLobbySuccessCommand.FromNetworkMessage;
                break;
            case JoinedLobbyFailMessageable.MSGTYPE:
                converterFunc = JoinedLobbyFailCommand.FromNetworkMessage;
                break;
            case CreatedLobbySuccessMessageable.MSGTYPE:
                converterFunc = CreatedLobbySuccessCommand.FromNetworkMessage;
                break;
            case CreatedLobbyFailMessageable.MSGTYPE:
                converterFunc = CreatedLobbyFailCommand.FromNetworkMessage;
                break;
            case CityUpdateMessageable.MSGTYPE:
                converterFunc = CityUpdateCommand.FromNetworkMessage;
                break;
            case TripUpdateMessegable.MSGTYPE:
                converterFunc = TripUpdateCommand.FromNetworkMessage;
                break;
            case PlayerJoinedMessageable.MSGTYPE:
                converterFunc = PlayerJoinedCommand.FromNetworkMessage;
                break;
            case ScoreUpdateMessageable.MSGTYPE:
                converterFunc = ScoreUpdateCommand.FromNetworkMessage;
                break;
            case StartGameMessageable.MSGTYPE:
                converterFunc = StartGameCommand.FromNetworkMessage;
                break;

        }

        return converterFunc?.Invoke(message);
    }

    private static void QueueCommand(ICommand command, string name)
    {
        if (command != null)
            CommandQueue.Queue(command, name);
        //else
            //UnityEngine.Debug.Log(name + " is null");
    }

    /*
    // just leaving some thoughts here:

    private static Dictionary<string, MessageToCommandFunction> converterFuncs;

    static NetworkMessageHandler()
    {
        converterFuncs = new Dictionary<string, MessageToCommandFunction>();
    }

    public static void AddHandler(string MSGTYPE, MessageToCommandFunction converterFunc)
    {
        if (!converterFuncs.ContainsKey(MSGTYPE))
            converterFuncs.Add(MSGTYPE, converterFunc);
    }
    private static INetworkCommand ConvertToCommand(NetworkMessage message)
    {
        bool found = converterFuncs.TryGetValue(message.typeName, out MessageToCommandFunction foundConverterFunc);

        // maybe too much (instead of func?.Invoke())
        // but safer if value type doesn't default to null
        return found ? foundConverterFunc.Invoke(message) : null;
    }

    */
}