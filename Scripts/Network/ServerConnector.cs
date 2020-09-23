using System.Net;
using System.Net.Sockets;

public class ServerConnector
{
    public static readonly int port = 443;
    public static readonly int localPort = 40400;
    public static readonly IPAddress serverAddress = IPAddress.Parse("193.170.246.53");

    public static bool TryConnectToServer(out Socket socket)
    {
        return TryConnectAdress(serverAddress, out socket);
    }

    public static bool TryConnectToLocal(out Socket socket)
    {
        IPEndPoint ipe = new IPEndPoint(IPAddress.Loopback, localPort);
        return TryConnectTo(ipe, out socket);
    }

    private static bool TryConnectAdress(IPAddress address, out Socket socket)
    {
        if (TryConnectTo(new IPEndPoint(address, port), out socket))
        {
            return true;
        }
        socket = null;
        return false;
    }

    // method from: https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket?view=netcore-3.1
    private static bool TryConnect(string host, out Socket socket)
    {
        // Get host related information.
        IPHostEntry hostEntry = Dns.GetHostEntry(host);

        // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
        // an exception that occurs when the host IP Address is not compatible with the address family
        // (typical in the IPv6 case).
        foreach (IPAddress address in hostEntry.AddressList)
        {
            if (TryConnectTo(new IPEndPoint(address, port), out socket))
            {
                return true;
            }
        }
        socket = null;
        return false;
    }

    private static bool TryConnectTo(IPEndPoint ipe, out Socket socket)
    {
        socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            // todo: maybe make asynchronous
            socket.Connect(ipe);
        }
        catch(SocketException) { }

        return socket.Connected;
    }
}