using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoldierButton : MonoBehaviour
{
    private Button pressedButton = null;
    public bool buttonCheck;

    public GameObject[] soldierTypes;
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


                    Debug.Log(hit.collider.name);

                    if(pressedButton.name.Contains("dog"))
                    {
                        Instantiate(soldierTypes[0], hit.transform.position, hit.transform.rotation);
                    }
                    if(pressedButton.name.Contains("spearman"))
                    {
                        Instantiate(soldierTypes[1], hit.transform.position, hit.transform.rotation);
                        
                    }
                }
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
}
