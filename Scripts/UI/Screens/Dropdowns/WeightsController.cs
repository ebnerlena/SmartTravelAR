using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WeightsController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Text time, money, co2, culture;

    [SerializeField]
    private Slider timeSlider, moneySlider, co2Slider, cultureSlider;

    void Start()
    {
        SliderValueChanged(time, timeSlider.value);
        SliderValueChanged(money, moneySlider.value);
        SliderValueChanged(co2, co2Slider.value);
        SliderValueChanged(culture, cultureSlider.value);

        timeSlider.onValueChanged.AddListener((float value) => SliderValueChanged(time, value));
        moneySlider.onValueChanged.AddListener((float value) => SliderValueChanged(money, value));
        co2Slider.onValueChanged.AddListener((float value) => SliderValueChanged(co2, value));
        cultureSlider.onValueChanged.AddListener((float value) => SliderValueChanged(culture, value));
    }

    private void SliderValueChanged(Text text, float value)
    {
        text.text = value.ToString();
    }

    public void GetWeights(out Dictionary<Type, float> resourceWeights, out float daysLeftWeight, out float culturePointWeight)
    {
        resourceWeights =
                new Dictionary<Type, float>
                {
                    { typeof(MoneyResource), moneySlider.value },
                    { typeof(CO2Resource), co2Slider.value }
                };
        culturePointWeight = cultureSlider.value;
        daysLeftWeight = timeSlider.value;
    }

    public void Reset()
    {
        timeSlider.value = 1;
        moneySlider.value = 1;
        co2Slider.value = 1;
        cultureSlider.value = 1;
    }
}
