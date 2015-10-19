using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel;
    public Button lockInButton;
    public TimerController timer;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
            lockInButton.interactable = !lockInButton.interactable;
            timer.paused = !timer.paused;
        }
    }

    public void exitToMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }
}
