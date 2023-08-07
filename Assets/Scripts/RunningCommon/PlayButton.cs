using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    private bool isButtonPressed = false;

    public bool GetIsButtonPressed()
    {
        return (isButtonPressed);
    }

    public void PressButton()
    {
        isButtonPressed = true;
        this.gameObject.SetActive(false);
    }
}
