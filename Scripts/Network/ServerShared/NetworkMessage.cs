using System;

[Serializable()]
public class NetworkMessage
{
    public string typeName;
    public string body;

    // public default constructor is required for XmlSerialization
    public NetworkMessage() : this(string.Empty, string.Empty) { }

    public NetworkMessage(string typeName, string body)
    {
        this.typeName = typeName;
        this.body = body;
    }
}