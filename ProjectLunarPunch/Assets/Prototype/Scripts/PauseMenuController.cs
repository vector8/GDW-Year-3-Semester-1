using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
        }
    }

    public void exitToMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }
}
