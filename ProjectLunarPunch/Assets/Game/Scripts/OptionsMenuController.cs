using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    public GameObject optionsPanel;

    public void exitToMainMenu()
    {
        BattleManager.logBattle("Battle finished!\n");

        Application.LoadLevel("MainMenu");
    }

    public void restart()
    {
        BattleManager.logBattle("Battle finished!\n");
        Application.LoadLevel(Application.loadedLevel);
    }

    public void toggleOptionsMenu()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }
}
