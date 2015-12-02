using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    public GameObject optionsPanel;

    public void exitToMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    public void restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void toggleOptionsMenu()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }
}
