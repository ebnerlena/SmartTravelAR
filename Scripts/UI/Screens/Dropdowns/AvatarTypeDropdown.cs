using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AvatarTypeDropdown: MonoBehaviour
{
    Dropdown avatarType;

    void Start()
    {

        avatarType = GetComponent<Dropdown>();
        avatarType.ClearOptions();
        List<string> at_Options = new List<string>();

        foreach (string typeName in Enum.GetNames(typeof(AvatarType)))
        {
            at_Options.Add(typeName);
           
        }
        avatarType.AddOptions(at_Options);
    }
}
