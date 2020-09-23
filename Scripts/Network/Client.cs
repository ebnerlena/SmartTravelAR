using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class Client : IObservable<NetworkMessage>
{    
    public static readonly int bufferSize = 2048;
    public static readonly int sendTickRateMS = 1000 / 50;
    public List<IObserver<NetworkMessage>> observers { get; private set; }

    private Socket socket;
    private Queue<byte[]> sendQueue;
    private bool sendQueueIsWorking;

    private bool nextIsBig;

    public Client()
    {
        observers = new List<IObserver<NetworkMessage>>();

        sendQueue = new Queue<byte[]>();
        sendQueueIsWorking = false;

        nextIsBig = false;
    }


    public void Share(NetworkMessage message)
    {
        Share(Serializer.Serialize(message));
    }

    public void OnReceive(byte[] data)
    {
        NetworkMessage message = Serializer.Deserialize(data);
        if (message != null)
        {
            NotifyObservers(message);
        }
        if (nextIsBig)
        {
            nextIsBig = false;
            BufferSizeBackToNormal();
        }
    }

    #region connection methods
    public bool ConnectByNetworkStatus(NetworkStatus status)
    {
        bool connected = false;
        Socket socket = null;

        switch (status)
        {
            case NetworkStatus.LocalNetwork:
                connected = ServerConnector.TryConnectToLocal(out socket);
                break;
            case NetworkStatus.Offline:
                break;
            default:
            case NetworkStatus.Online:
                connected = ServerConnector.TryConnectToServer(out socket);
                break;
        }
        if (connected && socket != null)
            SetupConnected(socket);

        return connected;
    }

    private void SetupConnected(Socket connectSocket)
    {
        this.socket = connectSocket;
        this.socket.ReceiveBufferSize = bufferSize;
        this.socket.SendBufferSize = bufferSize;

        new Task(ListenForNext).Start();
    }

    public void Disconnect()
    {
        if (socket != null)
        {
            // activley blocking thread until sending is done
            while (sendQueueIsWorking)
            {
                Thread.Sleep(sendTickRateMS);
            }
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
    #endregion

    #region buffer size
    public void SetNextBufferSize(int size)
    {
        if(size > 0)
        {
            nextIsBig = true;
            socket.ReceiveBufferSize = size;
            //socket.SendBufferSize = size;
        }
    }

    private void BufferSizeBackToNormal()
    {
        socket.ReceiveBufferSize = bufferSize;
        //socket.SendBufferSize = bufferSize;
    }
    #endregion

    #region listen
    public void ListenForNext()
    {
        while (socket != null && socket.Connected)
        {
            byte[] data = new byte[bufferSize];
            try
            {
                // blocks task and throws exception if disconnected
                // so try/catch and not if(socket.Connected)
                // todo: break receive if other connection method was chosen
                int read = socket.Receive(data);
                if (read > 0)
                {
                    // new Task so i can listen for next package
                    new Task(() => OnReceive(data)).Start();
                }
                else
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    socket = null;
                }
            }
            catch (SocketException) { }
        }
    }
    #endregion

    #region share and sendQueue
    private void Share(byte[] bytes)
    {
        sendQueue.Enqueue(bytes);
        StartSendQueue();
    }

    private void StartSendQueue()
    {
        new Task(WorkSendQueue).Start();
    }

    private async void WorkSendQueue()
    {
        if (!sendQueueIsWorking)
        {
            sendQueueIsWorking = true;
            while (sendQueue.Count > 0)
            {
                if (socket.Connected)
                    socket.Send(sendQueue.Dequeue());
                else
                    break;

                await Task.Delay(sendTickRateMS);
            }
            sendQueueIsWorking = false;
        }
    }
    #endregion

    #region observer methods
    public void AddObserver(IObserver<NetworkMessage> observer)
    {
        if (!observers.Contains(observer)) { observers.Add(observer); }
    }

    public void RemoveObserver(IObserver<NetworkMessage> observer)
    {
        if (observers.Contains(observer)) { observers.Remove(observer); }
    }

    public void NotifyObservers(NetworkMessage message)
    {
        //UnityEngine.Debug.Log("received msg: " + message.typeName + message.body);
        // not using foreach, since observers can remove themselves on execution
        // and modify observers list
        for (int i = observers.Count - 1; i >= 0; i--)
        {
            observers[i].ObserverUpdate(message);
        }
    }
    #endregion
}