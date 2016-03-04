using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ButtonClass : System.Object
{
    public Button m_spawnWarHound,
        m_spawnSpearMan,
        m_spawnPaladin,
        m_spawnArcher,
        m_spawnBerserker,
        m_spawnSwordsman;
}

public enum ButtonNames
{
    NONE, DOG, SPEARMAN, ARCHER, BERSERKER, SWORDSMAN, PALADIN
}

public class SoldierButton : MonoBehaviour
{
    public int ID;

    [SerializeField]
    private ButtonClass spawnButtons;

    public SplineWalker walker = GameObject.Find("Main Camera").GetComponent<SplineWalker>();

    private Button pressedButton = null;
    public bool buttonCheck;

    public GameObject[] soldierTypes;
    public GameObject[] g;

    public Button goBtn;
    private ButtonNames pressedButtonName = ButtonNames.NONE;

    private List<Stats_UI_Handler> m_statsHandlers;


    // Add on click listeners, for each button
    void Start()
    {
        // Initialize the list for the UI Handler
        m_statsHandlers = new List<Stats_UI_Handler>();

        spawnButtons.m_spawnWarHound.onClick.AddListener(() =>
            ButtonPressListener(ButtonNames.DOG));
        spawnButtons.m_spawnArcher.onClick.AddListener(() =>
            ButtonPressListener(ButtonNames.ARCHER));
        spawnButtons.m_spawnBerserker.onClick.AddListener(() =>
            ButtonPressListener(ButtonNames.BERSERKER));
        spawnButtons.m_spawnPaladin.onClick.AddListener(() =>
            ButtonPressListener(ButtonNames.PALADIN));
        spawnButtons.m_spawnSpearMan.onClick.AddListener(() =>
            ButtonPressListener(ButtonNames.SPEARMAN));
        spawnButtons.m_spawnSwordsman.onClick.AddListener(() =>
            ButtonPressListener(ButtonNames.SWORDSMAN));

        // Set the GameObject array to null, 
        // This is the list of all units
        for (int i = 0; i < 5; i++)
        {
            g[i] = null;
        }
    }

    // This listener will only happen when you click on the button
    public void ButtonPressListener(ButtonNames a_name)
    {
        // It changes the current button that was pressed. This info
        // is used inside of the MouseClicked() function
        pressedButtonName = a_name;
    }

    // This will shoot a mouse clicked function, if you click the left mouse
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Call the function
            MouseClicked();
        }
    }

    // This will go through all canvases that were created, and turn them off. This
    // is used for convenience purposes, instead of manually turning each one off
    void TurnOffCanvases()
    {
        // Return if there are no canvases
        if (m_statsHandlers.Count == 0)
            return;

        // For each StatsHandle, call the ActiveUI Function, with value
        // false. This turns each Canvas off
        foreach (Stats_UI_Handler sHndl in m_statsHandlers)
        {
            sHndl.gameObject.SendMessage("activeUI", false);
        }
    }

    // When the user clicks the mouse, Update calls this function
    void MouseClicked()
    {
        // If the user never clicked on a button, exit the function
        if (pressedButtonName == ButtonNames.NONE)
            return;

        // Cast a ray from the mouse position, and get the info about the collider hit
        Ray ray = Camera.main.ScreenPointToRay(MouseInputWrapper.getMousePosition());
        RaycastHit hit;

        // If there was a hit
        if (Physics.Raycast(ray, out hit))
        {
            // And if it is a SpawnShere object (because of the tag)
            if (hit.collider.tag == "SpawnSphere")
            {
                // Turn off all the canvases
                TurnOffCanvases();
                // Set the ID to the spot value in the SpawnSphere gameObject, 
                // using the value from Stats_UI_Handler
                ID = hit.collider.gameObject.GetComponent<Stats_UI_Handler>().spotValue;

                // Get a handle to the Stats_UI_Handler class
                Stats_UI_Handler sh = hit.collider.gameObject.GetComponent<Stats_UI_Handler>();
                // Activate the Canvas
                sh.SendMessage("activeUI", true);

                // If the list of stats handlers doesn't contain the clicked handler, 
                // then add it to the list
                if (!m_statsHandlers.Contains(sh))
                    m_statsHandlers.Add(sh);

                // for the current value pressedButtonName (it is an 
                // enum of names)
                switch (pressedButtonName)
                {
                    // Check if it is a DOG, and then destroy the current
                        // object that is in that location on the map.
                        // Instantiate a new solider of the current type DOG (0)
                        // at the position you clicked, with the rotation of the
                        // object you clicked on
                    case ButtonNames.DOG:
                        {
                            Destroy(g[ID]);
                            g[ID] = (GameObject)Instantiate(soldierTypes[0], hit.transform.position, hit.transform.rotation);
                        }
                        break;
                        // Do the same for all objects
                    case ButtonNames.SPEARMAN:
                        {
                            Destroy(g[ID]);
                            g[ID] = (GameObject)Instantiate(soldierTypes[1], hit.transform.position, hit.transform.rotation);
                        }
                        break;
                    case ButtonNames.SWORDSMAN:
                        {
                            Destroy(g[ID]);
                            g[ID] = (GameObject)Instantiate(soldierTypes[2], hit.transform.position, hit.transform.rotation);
                        }
                        break;
                    case ButtonNames.ARCHER:
                        {
                            Destroy(g[ID]);
                            g[ID] = (GameObject)Instantiate(soldierTypes[3], hit.transform.position, hit.transform.rotation);
                        }
                        break;
                    case ButtonNames.BERSERKER:
                        {
                            Destroy(g[ID]);
                            g[ID] = (GameObject)Instantiate(soldierTypes[4], hit.transform.position, hit.transform.rotation);
                        }
                        break;
                    case ButtonNames.PALADIN:
                        {
                            Destroy(g[ID]);
                            g[ID] = (GameObject)Instantiate(soldierTypes[5], hit.transform.position, hit.transform.rotation);
                        }
                        break;
                }
            }
        }
        
        // if you want to reset the button to none each time you click, uncomment this
        //pressedButtonName = ButtonNames.NONE;
    }
    
    public void loadBattleScreen()
    {
        string log = "(" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ") Player chose: ";

        for (int i = 0; i < 5; i++)
        {
            Unit u = g[i].GetComponent<Unit>();
            ArmyConfiguration.army[i] = u.type;
            log += Enum.GetName(typeof(ClassType), u.type) + ", ";
        }
        log += "\n";

        string path = Application.persistentDataPath.Replace('/', '\\');
        FileIOWrapper.saveFile(path + @"\ArmyBuilderLog.b", log, true, true);

       // walker.enabled = true;
        
        Application.LoadLevel("Battle");
    }

    public void backToMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }
}
