using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Runtime.InteropServices;

public struct UnitCommand
{
    public Unit fromUnit, toUnit;
    public LineRenderer line;
    public int crit;
}

public class UnitControl : MonoBehaviour
{
    public GameObject linePrefab;
    public BattleManager battleMgr;
    public Button lockInButton;
    public TimerController timerControl;
    public int AILevel = 0;

    public bool online = true;
    public bool isHost = true;
    public string serverIP = "10.120.35.53";

    // TODO: remove after networking?
    public GameObject[] prefabs = new GameObject[6];
    public GameObject[] allySpots = new GameObject[5], enemySpots = new GameObject[5];
    public Unit[] enemies = new Unit[5], allies = new Unit[5];

    private int error;
    private bool drawingLine = false, commandExists = false, endDrawing = false;
    private Vector3 lineStart, lineStartScreenSpace;
    private Vector3 lineEnd, lineEndScreenSpace;
    private UnitCommand currentCommand;
    private List<UnitCommand> playerCommands = new List<UnitCommand>();

    private const int NUM_LINE_VERTICES = 20;

    void Start()
    {
        online = true;
        isHost = true;
        if (online)
        {
            if (isHost)
            {
                error = NetworkWrapper.initializeServer();
                if (error != 0)
                {
                    NetworkWrapper.closeConnections();
                    Application.LoadLevel("MainMenu");
                }
            }
            else
            {
                NetworkWrapper.setServer(serverIP);
                error = NetworkWrapper.initializeClient();
                if (error != 0)
                {
                    NetworkWrapper.closeConnections();
                    Application.LoadLevel("MainMenu");
                }
            }
        }

        BattleManager.logBattle("BATTLE STARTING!!\n");

        string sendingMsg = "AC";

        // Initialize allies using ArmyConfiguration and enemies using random creation
        for (int i = 0; i < 5; i++)
        {
            int allyIndex;
            if (ArmyConfiguration.army[i] == ClassType.NotSet)
            {
                allyIndex = Random.Range(0, 5);
            }
            else
            {
                allyIndex = (int)ArmyConfiguration.army[i];
            }
            GameObject ally = Instantiate<GameObject>(prefabs[allyIndex]);
            allies[i] = ally.GetComponent<Unit>();
            allies[i].ally = true;
            ally.transform.SetParent(allySpots[i].transform, false);
            sendingMsg += allyIndex.ToString();
        }

        if (online)
        {
            error = NetworkWrapper.sendTo(sendingMsg);
            if (error != 0)
            {
                NetworkWrapper.closeConnections();
                Application.LoadLevel("MainMenu");
            }

            int timeOut = 100;
            error = NetworkWrapper.receive();
            while (error != 0)
            {
                timeOut -= 1;
                Thread.Sleep(200);
                if (timeOut <= 0)
                {
                    break;
                }
                error = NetworkWrapper.receive();
            }
            if (timeOut <= 0)
            {
                NetworkWrapper.closeConnections();
                Application.LoadLevel("MainMenu");

            }
            else
            {
                string receivedString = Marshal.PtrToStringAnsi(NetworkWrapper.readBuffer());
                NetworkWrapper.clearBuffer();
                if (receivedString.Substring(0, 2) == "AC")
                {
                    enemies[0] = Instantiate<GameObject>(prefabs[int.Parse(receivedString.Substring(2, 1))]).GetComponent<Unit>();
                    enemies[1] = Instantiate<GameObject>(prefabs[int.Parse(receivedString.Substring(3, 1))]).GetComponent<Unit>();
                    enemies[2] = Instantiate<GameObject>(prefabs[int.Parse(receivedString.Substring(4, 1))]).GetComponent<Unit>();
                    enemies[3] = Instantiate<GameObject>(prefabs[int.Parse(receivedString.Substring(5, 1))]).GetComponent<Unit>();
                    enemies[4] = Instantiate<GameObject>(prefabs[int.Parse(receivedString.Substring(6, 1))]).GetComponent<Unit>();
                }
            }

        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                int randomIndex = Random.Range(0, 5);
                GameObject enemy = Instantiate<GameObject>(prefabs[3]);
                enemies[i] = enemy.GetComponent<Unit>();
                enemy.transform.SetParent(enemySpots[i].transform, false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (battleMgr.isBattleInProgress())
        {
            return;
        }

        checkGameOver();

        if (battleMgr.isGameOver())
        {
            return;
        }

        if (Mathf.Approximately(timerControl.timer, 0f))
        {
            if (drawingLine)
            {
                if (commandExists && playerCommands.Contains(currentCommand))
                {
                    playerCommands.Remove(currentCommand);
                }
                DestroyImmediate(currentCommand.line.gameObject);
                drawingLine = false;
            }

            lockIn();
            return;
        }

        handleInput();

        if (playerCommands.Count > 0)
        {
            lockInButton.interactable = true;
        }
        else
        {
            lockInButton.interactable = false;
        }
    }

    private void checkGameOver()
    {
        // Loop through all units to determine if it is game over.
        bool enemiesDead = true, alliesDead = true;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && enemies[i].gameObject.activeSelf && !enemies[i].isDead())
            {
                enemiesDead = false;
                break;
            }
        }
        for (int i = 0; i < allies.Length; i++)
        {
            if (allies[i] != null && allies[i].gameObject.activeSelf && !allies[i].isDead())
            {
                alliesDead = false;
                break;
            }
        }

        // TODO: implement these screens
        if (alliesDead && enemiesDead)
        {
            battleMgr.setGameOver(GameOverType.Tie);
        }
        else if (alliesDead)
        {
            battleMgr.setGameOver(GameOverType.Lose);
        }
        else if (enemiesDead)
        {
            battleMgr.setGameOver(GameOverType.Win);
        }
    }

    private void handleInput()
    {
        bool mouseButtonDown = MouseInputWrapper.getMouseButtonDown(MouseInputWrapper.MouseButtons.Left);
        if (mouseButtonDown)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(MouseInputWrapper.getMousePosition());

            if (Physics.Raycast(ray, out hit))
            {
                Unit unit = hit.transform.gameObject.GetComponent<Unit>();
                if (unit != null && unit.ally)
                {
                    drawingLine = true;
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(hit.transform.GetComponentInChildren<Renderer>().bounds.center);
                    lineStart = getMouseWorldPosition(screenPos);
                    lineStartScreenSpace = screenPos;
                    //lineStart = hit.transform.position;

                    commandExists = false;
                    for (int i = 0; i < playerCommands.Count; i++)
                    {
                        if (playerCommands[i].fromUnit.transform.GetInstanceID() == hit.transform.GetInstanceID())
                        {
                            currentCommand = playerCommands[i];
                            commandExists = true;
                        }
                    }

                    if (!commandExists)
                    {
                        currentCommand = new UnitCommand();
                        GameObject lineObject = Instantiate<GameObject>(linePrefab);
                        currentCommand.line = lineObject.GetComponent<LineRenderer>();
                        Color c = new Color(Random.value, Random.value, Random.value, 0.5f);
                        currentCommand.line.SetColors(c, c);
                        currentCommand.fromUnit = unit;
                        lineObject.transform.parent = unit.transform;
                    }
                }
            }
        }
        else if (!MouseInputWrapper.getMouseButton(MouseInputWrapper.MouseButtons.Left) && drawingLine)
        {
            drawingLine = false;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(MouseInputWrapper.getMousePosition());

            bool clearCommand = true;

            if (Physics.Raycast(ray, out hit))
            {
                Unit unit = hit.transform.gameObject.GetComponent<Unit>();

                if (unit != null && !unit.ally)
                {
                    clearCommand = false;
                    drawingLine = true;
                    endDrawing = true;

                    Vector3 screenPos = Camera.main.WorldToScreenPoint(hit.transform.GetComponentInChildren<Renderer>().bounds.center);
                    lineEnd = getMouseWorldPosition(screenPos);
                    lineEndScreenSpace = screenPos;

                    currentCommand.toUnit = unit;
                    currentCommand.crit = Random.Range(0, 100);
                    if (!commandExists)
                    {
                        playerCommands.Add(currentCommand);
                    }
                }
            }

            if (clearCommand)
            {
                if (commandExists)
                {
                    if (playerCommands.Contains(currentCommand))
                    {
                        playerCommands.Remove(currentCommand);
                    }
                }
                DestroyImmediate(currentCommand.line.gameObject);
            }

        }

        if (drawingLine)
        {
            Vector3 worldEndPos;
            Vector3 endScreenSpace;
            if (!endDrawing)
            {
                worldEndPos = getMouseWorldPosition();
                endScreenSpace = MouseInputWrapper.getMousePosition();
            }
            else
            {
                worldEndPos = lineEnd;
                endScreenSpace = lineEndScreenSpace;
            }

            Vector3 anchor1 = Camera.main.ScreenToWorldPoint(new Vector3(lineStartScreenSpace.x + (7f / 16f) * (endScreenSpace.x - lineStartScreenSpace.x), Camera.main.pixelHeight * 3f / 4f, Camera.main.nearClipPlane + 10f));
            Vector3 anchor2 = Camera.main.ScreenToWorldPoint(new Vector3(lineStartScreenSpace.x + (9f / 16f) * (endScreenSpace.x - lineStartScreenSpace.x), Camera.main.pixelHeight * 3f / 4f, Camera.main.nearClipPlane + 10f));

            //Vector3 anchor1 = new Vector3(-2.5f, 1f, 13f);
            //Vector3 anchor2 = new Vector3(2.5f, 1f, 13f);

            for (int i = 0; i < NUM_LINE_VERTICES; i++)
            {
                Vector3 pos = new Vector3();

                // set pos based on bezier function of startpos, endpos, and two anchor points at centre of screen.
                float u = (float)i / (float)(NUM_LINE_VERTICES - 1);

                pos = bezier(u, lineStart, anchor1, anchor2, worldEndPos);

                currentCommand.line.SetPosition(i, pos);
            }

            if (endDrawing)
            {
                drawingLine = false;
                endDrawing = false;
            }
        }
    }

    private Vector3 bezier(float u, Vector3 p0, Vector3 c0, Vector3 c1, Vector3 p1)
    {
        return (u * u * u) * (-p0 + 3f * c0 - 3f * c1 + p1) +
            (u * u) * (3f * p0 - 6f * c0 + 3f * c1) +
            (u) * (-3f * p0 + 3f * c0) +
            p0;

        //return (1f - u) * (1f - u) * (1f - u) * p0 + 3f * (1f - u) * (1f - u) * u * c0 + 3f * (1 - u) * u * u * c1 + u * u * u * p1;
    }

    private Vector3 getMouseWorldPosition()
    {
        Vector3 mousePos = MouseInputWrapper.getMousePosition();
        mousePos.z = Camera.main.nearClipPlane + 10f;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

    private Vector3 getMouseWorldPosition(Vector3 screenPosition)
    {
        Vector3 mousePos = new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane + 10f);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

    public void lockIn()
    {
        //// Battle calculations and animations here. ////
        BattleManager.logBattle("New Round!\n");
        List<UnitCommand> allCommands = new List<UnitCommand>();

        // TODO: Generate or fetch enemy commands here.
        List<int> availableUnits = new List<int>();
        for (int i = 0; i < allies.Length; i++)
        {
            if (allies[i] != null && allies[i].gameObject.activeSelf && !allies[i].isDead())
            {
                availableUnits.Add(i);
            }
        }
        if (availableUnits.Count > 0 && !online)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null && enemies[i].gameObject.activeSelf && !enemies[i].isDead())
                {
                    UnitCommand cmd = new UnitCommand();
                    cmd.fromUnit = enemies[i];
                    int unitNumber = Random.Range(0, availableUnits.Count - 1);
                    if (AILevel == 0)
                    {
                        cmd.toUnit = allies[availableUnits[unitNumber]];
                    }
                    else if (AILevel == 1)
                    {
                        float lowestHP = 100000;
                        // so like, you put a fucking colon thing over here, make a function over there, donezo.
                        int lowestHPAlly = 0;
                        for (int k = 0; k < allies.Length; k++)
                        {
                            if (allies[k].hp < lowestHP && allies[k].isDead() == false)
                            {
                                Debug.Log("found");
                                lowestHP = allies[k].hp;
                                lowestHPAlly = k;
                            }
                        }
                        cmd.toUnit = allies[lowestHPAlly];
                    }
                    else if (AILevel == 2)
                    {
                        float lowestEffectiveHP = 100000;
                        int lowestHPAlly = 0;
                        for (int k = 0; k < allies.Length; k++)
                        {
                            if (allies[k].hp * 100 / (100 + allies[k].def) < lowestEffectiveHP && allies[k].isDead() == false)
                            {
                                lowestEffectiveHP = allies[k].hp * 100 / (100 + allies[k].def);
                                lowestHPAlly = k;
                            }
                        }
                        cmd.toUnit = allies[lowestHPAlly];
                    }
                    cmd.crit = Random.Range(0, 100);
                    allCommands.Add(cmd);
                }
            }
        }
        else
        {
            // game should be over already...
        }

        string msgToSend = "CM";
        Random rand = new Random();
        // Loop through player commands to destroy all lines.
        for (int i = 0; i < playerCommands.Count; i++)
        {
            DestroyImmediate(playerCommands[i].line.gameObject);

            msgToSend += playerCommands[i].fromUnit.ToString() + playerCommands[i].toUnit.ToString() + playerCommands[i].crit.ToString().PadLeft(3, '0');
        }

        if (online)
        {
            error = NetworkWrapper.sendTo(msgToSend);

            if (error != 0)
            {
                NetworkWrapper.closeConnections();
                Application.LoadLevel("MainMenu");
            }

            int timeOut = 100;
            while (NetworkWrapper.receive() != 0)
            {
                timeOut -= 1;
                Thread.Sleep(100);
                if (timeOut <= 0)
                {
                    break;
                }
            }
            if (timeOut <= 0)
            {
                NetworkWrapper.closeConnections();
                Application.LoadLevel("MainMenu");
            }
            else
            {
                string receivedString = Marshal.PtrToStringAnsi(NetworkWrapper.readBuffer());
                NetworkWrapper.clearBuffer();

                if (receivedString.Substring(0, 2) == "AC")
                {
                    if (receivedString.Length >= 7)
                    {
                        UnitCommand onlineEnemyCmd = new UnitCommand();
                        onlineEnemyCmd.fromUnit = enemies[int.Parse(receivedString.Substring(2, 1))];
                        onlineEnemyCmd.toUnit = allies[int.Parse(receivedString.Substring(3, 1))];
                        onlineEnemyCmd.crit = int.Parse(receivedString.Substring(4, 3));
                        allCommands.Add(onlineEnemyCmd);
                    }
                    if (receivedString.Length >= 12)
                    {
                        UnitCommand onlineEnemyCmd = new UnitCommand();
                        onlineEnemyCmd.fromUnit = enemies[int.Parse(receivedString.Substring(7, 1))];
                        onlineEnemyCmd.toUnit = allies[int.Parse(receivedString.Substring(8, 1))];
                        onlineEnemyCmd.crit = int.Parse(receivedString.Substring(9, 3));
                        allCommands.Add(onlineEnemyCmd);
                    }
                    if (receivedString.Length >= 17)
                    {
                        UnitCommand onlineEnemyCmd = new UnitCommand();
                        onlineEnemyCmd.fromUnit = enemies[int.Parse(receivedString.Substring(12, 1))];
                        onlineEnemyCmd.toUnit = allies[int.Parse(receivedString.Substring(13, 1))];
                        onlineEnemyCmd.crit = int.Parse(receivedString.Substring(14, 3));
                        allCommands.Add(onlineEnemyCmd);
                    }
                    if (receivedString.Length >= 22)
                    {
                        UnitCommand onlineEnemyCmd = new UnitCommand();
                        onlineEnemyCmd.fromUnit = enemies[int.Parse(receivedString.Substring(17, 1))];
                        onlineEnemyCmd.toUnit = allies[int.Parse(receivedString.Substring(18, 1))];
                        onlineEnemyCmd.crit = int.Parse(receivedString.Substring(19, 3));
                        allCommands.Add(onlineEnemyCmd);
                    }
                    if (receivedString.Length >= 27)
                    {
                        UnitCommand onlineEnemyCmd = new UnitCommand();
                        onlineEnemyCmd.fromUnit = enemies[int.Parse(receivedString.Substring(22, 1))];
                        onlineEnemyCmd.toUnit = allies[int.Parse(receivedString.Substring(23, 1))];
                        onlineEnemyCmd.crit = int.Parse(receivedString.Substring(24, 3));
                        allCommands.Add(onlineEnemyCmd);
                    }
                }

            }
        }

        allCommands.AddRange(playerCommands);



        // Sort commands by speed.
        allCommands.Sort(delegate(UnitCommand a, UnitCommand b)
        {
            if (a.fromUnit.type == ClassType.Archer && b.fromUnit.type != ClassType.Archer)
            {
                return -1;
            }
            else if (b.fromUnit.type == ClassType.Archer && a.fromUnit.type != ClassType.Archer)
            {
                return 1;
            }
            else
            {
                return -a.fromUnit.speed.CompareTo(b.fromUnit.speed);
            }
        });

        Queue<UnitCommand> sortedCommands = new Queue<UnitCommand>();

        // Loop through allCommands to create queue for BattleManager and to destroy all lines.
        for (int i = 0; i < allCommands.Count; i++)
        {
            sortedCommands.Enqueue(allCommands[i]);
        }



        battleMgr.commenceBattle(sortedCommands);

        playerCommands.Clear();
    }
}
