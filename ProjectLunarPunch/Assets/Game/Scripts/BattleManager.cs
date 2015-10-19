using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public Button lockInButton;
    public Text instructionText, timerText;
    public TimerController timerController;

    private enum BattleState
    {
        NotInProgress,
        Attacking,
        Waiting,
        Cleanup
    }

    private BattleState state = BattleState.NotInProgress;
    private Queue<UnitCommand> currentCommands;

    private Timer waitTimer = new Timer();

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
                        waitTimer.setTime(1f);
                        state = BattleState.Waiting;
                    }
                    else
                    {
                        state = BattleState.Cleanup;
                    }
                }
                break;
            case BattleState.Waiting:
                if(waitTimer.isDone())
                {
                    state = BattleState.Cleanup;
                }
                else
                {
                    waitTimer.update(Time.deltaTime);
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
}
