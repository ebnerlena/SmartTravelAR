using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NextCityDropdown : MonoBehaviour
{
    Dropdown availableCities;
    public TransportTypeDropdown transportType;

    private List<string> cities = new List<string>();

    void Awake()
    {
        availableCities = GetComponent<Dropdown>();
    }
    void Start()
    {
        availableCities.ClearOptions();

        cities = GraphGenerator.GetCityOptions(GameManager.Instance.Player.Trip.CurrentCity);
        availableCities.AddOptions(cities);

        availableCities.onValueChanged.AddListener(delegate
        {
            OnValueChanged(availableCities);
        });

        OnValueChanged(availableCities);
    }

    void OnValueChanged(Dropdown change)
    {

        transportType.UpdateTransportTypes(this.cities[change.value]);
    }

    public void UpdateCityOptions()
    {
        availableCities = GetComponent<Dropdown>();
        availableCities.ClearOptions();

        cities = GraphGenerator.GetCityOptions(GameManager.Instance.Player.Trip.CurrentCity);

        if (cities.Count != 0)
        {
            availableCities.AddOptions(cities);
            OnValueChanged(availableCities);
        }
        else
        {
            Debug.Log("Game-Over");
            GameManager.Instance.GameOver();
        }
            
    }

}
