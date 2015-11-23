using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoldierButton : MonoBehaviour
{
    private Button pressedButton = null;
    public bool buttonCheck;

    public GameObject[] soldierTypes;
    public GameObject[] g;

    public Button goBtn;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (pressedButton != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "SpawnSphere" && Input.GetMouseButtonDown(0))
                {

                    char spawnLocID = hit.collider.name.ToCharArray()[0];
                    int ID = (int)spawnLocID - 49;

                    Debug.Log("sphere " + hit.collider.name);
                    Debug.Log("ID " + ID);

                    if (pressedButton.name.Contains("dog"))
                    {

                        Destroy(g[ID]);
                        g[ID] = (GameObject)Instantiate(soldierTypes[0], hit.transform.position, hit.transform.rotation);


                    }
                    if (pressedButton.name.Contains("spearman"))
                    {
                        Destroy(g[ID]);
                        g[ID] = (GameObject)Instantiate(soldierTypes[1], hit.transform.position, hit.transform.rotation);

                    }
                    if (pressedButton.name.Contains("swordsman"))
                    {
                        Destroy(g[ID]);
                        g[ID] = (GameObject)Instantiate(soldierTypes[2], hit.transform.position, hit.transform.rotation);

                    }
                    if (pressedButton.name.Contains("archer"))
                    {
                        Destroy(g[ID]);
                        g[ID] = (GameObject)Instantiate(soldierTypes[3], hit.transform.position, hit.transform.rotation);

                    }
                    if (pressedButton.name.Contains("berserker"))
                    {
                        Destroy(g[ID]);
                        g[ID] = (GameObject)Instantiate(soldierTypes[4], hit.transform.position, hit.transform.rotation);

                    }
                    if (pressedButton.name.Contains("horseman"))
                    {
                        Destroy(g[ID]);
                        g[ID] = (GameObject)Instantiate(soldierTypes[5], hit.transform.position, hit.transform.rotation);

                    }
                }
            }
        }

        goBtn.interactable = true;
        for (int i = 0; i < 5; i++)
        {
            if (g[i] == null)
            {
                goBtn.interactable = false;
                break;
            }
        }
    }

    public void buttonPress(Button caller)
    {
        Debug.Log(caller.name + " is active");
        if (pressedButton != null)
        {
            pressedButton.interactable = true;
            buttonCheck = pressedButton.IsInteractable();
        }



        pressedButton = caller;
        pressedButton.interactable = false;
        buttonCheck = pressedButton.IsInteractable();
    }

    public void loadBattleScreen()
    {
        for (int i = 0; i < 5; i++)
        {
            Unit u = g[i].GetComponent<Unit>();
            ArmyConfiguration.army[i] = u.type;
        }

        Application.LoadLevel("Battle");
    }
}
