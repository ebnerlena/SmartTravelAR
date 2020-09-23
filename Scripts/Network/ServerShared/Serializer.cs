using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

public class Serializer
{
    // using XmlSerializer instead of binary
    // so that file / class can be easily shared
    // and doesn't have to exactly be the same assembly
    public static byte[] Serialize<T>(T message) where T : NetworkMessage
    {
        using (MemoryStream stream = new MemoryStream())
        {
            new XmlSerializer(typeof(T)).Serialize(stream, message);
            return stream.ToArray();
        }
    }

    public static T Deserialize<T>(byte[] bytes) where T : NetworkMessage
    {
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            try
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(stream);
            }
            catch (SerializationException se)
            {
                Console.WriteLine(se.Message);
                return null;
            }
        }
    }

    public static NetworkMessage Deserialize(byte[] bytes) { return Deserialize<NetworkMessage>(bytes); }
    public static byte[] Serialize(NetworkMessage message) { return Serialize<NetworkMessage>(message); }
    public static byte[] Serialize(INetworkMessageable messageable) { return Serialize(messageable.ToNetworkMessage()); }
}