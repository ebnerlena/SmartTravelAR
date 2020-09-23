using System;

[Serializable]
public class PlayerInfo
{
    public string id;
    public string name;
    public string avatarType;

    public PlayerInfo(string id, string name, string avatarType)
    {
        this.id = id;
        this.name = name;
        this.avatarType = avatarType;
    }
}