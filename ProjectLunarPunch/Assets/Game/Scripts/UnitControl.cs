using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public struct UnitCommand
{
    public Unit fromUnit, toUnit;
    public LineRenderer line;
}

public class UnitControl : MonoBehaviour
{
    public GameObject linePrefab;
    public BattleManager battleMgr;
    public Button lockInButton;

    private bool drawingLine = false, commandExists = false, endDrawing = false;
    private Vector3 lineStart, lineStartScreenSpace;
    private Vector3 lineEnd, lineEndScreenSpace;
    private UnitCommand currentCommand;
    private List<UnitCommand> allCommands = new List<UnitCommand>();

    private const int NUM_LINE_VERTICES = 20;

    // Update is called once per frame
    void Update()
    {
        if(battleMgr.isBattleInProgress())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
                    for (int i = 0; i < allCommands.Count; i++)
                    {
                        if (allCommands[i].fromUnit.transform.GetInstanceID() == hit.transform.GetInstanceID())
                        {
                            currentCommand = allCommands[i];
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
        else if (!Input.GetMouseButton(0) && drawingLine)
        {
            drawingLine = false;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
                    if (!commandExists)
                    {
                        allCommands.Add(currentCommand);
                    }
                }
            }

            if (clearCommand)
            {
                if (commandExists)
                {
                    if (allCommands.Contains(currentCommand))
                    {
                        allCommands.Remove(currentCommand);
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
                endScreenSpace = Input.mousePosition;
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

        if(allCommands.Count > 0)
        {
            lockInButton.interactable = true;
        }
        else
        {
            lockInButton.interactable = false;
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
        Vector3 mousePos = Input.mousePosition;
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
        print("BATTLE STARTING!!");

        // TODO: Generate or fetch enemy commands here.

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
            Destroy(allCommands[i].line.gameObject);
            sortedCommands.Enqueue(allCommands[i]);
        }

        battleMgr.commenceBattle(sortedCommands);

        allCommands.Clear();
    }
}
