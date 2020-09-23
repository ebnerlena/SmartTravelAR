using System;
using System.Collections.Generic;

public class PlayerResource
{
    public string PlayerId { get; }
    public string PlayerName { get; }

    public Dictionary<Type, Resource> Resources { get; private set; }

    public PlayerResource(Player player)
    {
        this.PlayerName = player.Name;
        this.Resources = player.Resources;
    }
}
