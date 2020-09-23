using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TransportTypeDropdown : MonoBehaviour
{
    Dropdown transportTypes;
    private string selectedCity;
    List<string> transports;
    void Awake()
    {
        transportTypes = GetComponent<Dropdown>();
    }
    void Start()
    { 
        transportTypes.ClearOptions();
        transports = new List<string>();
        
        foreach (string type in Enum.GetNames(typeof(Transports)))
        {
            transports.Add(type);

        }
        transportTypes.AddOptions(transports);
    }

    public void UpdateTransportTypes(string to)
    {
        selectedCity = to;
        transportTypes = GetComponent<Dropdown>();
        transportTypes.ClearOptions();
        transports = GraphGenerator.GetTransportTypes(GameManager.Instance.Player.Trip.CurrentCity, to);
        transportTypes.AddOptions(transports);

    }

    public (string city, string transportType) GetSelection()
    {
        return (selectedCity, transports[transportTypes.value]);
    }
}
