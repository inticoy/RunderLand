using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedTextManager : MonoBehaviour
{
    [SerializeField] TMP_Text label;

    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();

        slider.onValueChanged.AddListener(UpdateText);
        UpdateText(slider.value);
    }

    public void UpdateText(float speed)
    {
        label.text = speed.ToString("0.0") + " km/h";
    }
}
