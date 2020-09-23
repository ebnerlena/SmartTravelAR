using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenderToggle : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Toggle female;

    [SerializeField]
    private Toggle male;

    [SerializeField]
    private Toggle neutral;

    private List<Toggle> genderChoices = new List<Toggle>();
    private Toggle active;

    void Awake()
    {
        genderChoices.Add(female);
        genderChoices.Add(male);
        genderChoices.Add(neutral);
        active = neutral;

        foreach (Toggle t in genderChoices)
        {
            t.onValueChanged.AddListener(delegate
            {
                OnValueChanged(t);
            });
        }
    }

    void OnValueChanged(Toggle change)
    {
        if (change.isOn)
            active = change;


        foreach(Toggle toggle in genderChoices)
        {
            if (toggle != active)
            {
                toggle.isOn = false;
            }
        }
    }
    
    public string GetSelected()
    {
        string gender = null;

        if (active == neutral)
            gender = "none";
        else if (active == female)
            gender = "female";
        else if (active == male)
            gender = "male";

        return gender;
    }
}
