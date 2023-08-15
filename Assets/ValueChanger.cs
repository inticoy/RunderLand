using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueChanger : MonoBehaviour
{
    public Slider slider;

    public void decreaseSpeed()
    {
        slider.value -= 0.5f;
        if (slider.value < 0)
            slider.value = 0;
    }

    public void increaseSpeed()
    {
        slider.value += 0.5f;
        if (slider.value >= 40)
            slider.value = 40;
    }
}
