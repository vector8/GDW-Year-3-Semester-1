using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoldierButton : MonoBehaviour
{
    private Button pressedButton = null;

    // Use this for initialization
    void Start()
    {
    }

    public void buttonPress(Button caller)
    {
        if(pressedButton != null)
        {
            pressedButton.interactable = true;
        }

        pressedButton = caller;
        pressedButton.interactable = false;
    }

    public bool getButtonState()
    {
        return pressedButton.IsInteractable();
    }
}
