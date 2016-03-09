using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public enum GameOverType
{
    Tie,
    Win,
    Lose
}

public class BattleManager : MonoBehaviour
{
    public Button lockInButton;
    public Text instructionText, timerText;
    public TimerController timerController;

    public GameObject gamePanel, gameOverPanel;
    public Text gameOverText;

    private enum BattleState
    {
        NotInProgress,
        Attacking,
        Waiting,
        Cleanup,
        GameOver
    }

    private BattleState state = BattleState.NotInProgress;
    private Queue<UnitCommand> currentCommands;

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BattleState.Attacking:
                {
                    UnitCommand command = currentCommands.Peek();
                    if (command.fromUnit.gameObject.activeSelf && command.toUnit.gameObject.activeSelf)
                    {
                        command.fromUnit.attack(command.toUnit);
                        state = BattleState.Waiting;
                    }
                    else
                    {
                        state = BattleState.Cleanup;
                    }
                }
                break;
            case BattleState.Waiting:
                if(!currentCommands.Peek().fromUnit.isAttacking())
                {
                    state = BattleState.Cleanup;
                }
                break;
            case BattleState.Cleanup:
                {
                    currentCommands.Dequeue();

                    if (currentCommands.Count == 0)
                    {
                        state = BattleState.NotInProgress;
                        lockInButton.interactable = true;
                        instructionText.enabled = true;
                        timerText.enabled = true;
                        timerController.paused = false;
                    }
                    else
                    {
                        state = BattleState.Attacking;
                    }
                }
                break;
        }
    }

    public void commenceBattle(Queue<UnitCommand> commands)
    {
        if(commands.Count > 0)
        {
            currentCommands = commands;
            state = BattleState.Attacking;
            lockInButton.interactable = false;
            instructionText.enabled = false;
            timerText.enabled = false;
            timerController.resetTimer();
            timerController.paused = true;
        }
    }

    public bool isBattleInProgress()
    {
        return state != BattleState.NotInProgress;
    }

    public void setGameOver(GameOverType type)
    {
        state = BattleState.GameOver;

        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        switch(type)
        {
            case GameOverType.Tie:
                gameOverText.text = "It's a tie!";
                break;
            case GameOverType.Win:
                gameOverText.text = "You win!";
                break;
            case GameOverType.Lose:
                gameOverText.text = "You lose!";
                break;
        }
    }

    public bool isGameOver()
    {
        return state == BattleState.GameOver;
    }

    public static void logBattle(string line)
    {
        line = "(" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ") " + line;
        string path = Application.persistentDataPath.Replace('/', '\\');
        FileIOWrapper.saveFile(path + @"\BattleLog.txt", line, false, true);
    }
}
