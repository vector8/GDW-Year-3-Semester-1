using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Stats_UI_Handler : MonoBehaviour {

    public GameObject statsPlane;

    public GameObject arrayhandle;

    public GameObject[] arry = new GameObject[5];
    public Canvas localHandle;

    public GameObject spot;

    public int spotValue;

    // This will turn the canvas on and off
    // The SoldierButton class calls with function with
    // GameObject.SendMessage("activeUI", true/false);
    public void activeUI(bool activate)
    {
        transform.GetChild(0).gameObject.SetActive(activate);
    }

	// Use this for initialization
	void Start () {
        // Get a handle to the SoliderButton List (it has the game objects
        // of all the units on the map)
       arry = arrayhandle.GetComponent<SoldierButton>().g;
	}
	
	// Update is called once per frame
    void Update()
    {
        // If the array isn't null, and the canvas is active
        if (arry[spotValue] != null && transform.GetChild(0).gameObject.activeInHierarchy)
        {
            // Set all of the text components in the canvas to the Stats of the current Unit
            // in that position
            localHandle.GetComponentsInChildren<Text>()[0].text = arry[spotValue].GetComponent<Unit>().hp.ToString();
            localHandle.GetComponentsInChildren<Text>()[1].text = arry[spotValue].GetComponent<Unit>().speed.ToString();
            localHandle.GetComponentsInChildren<Text>()[2].text = arry[spotValue].GetComponent<Unit>().att.ToString();
            localHandle.GetComponentsInChildren<Text>()[3].text = arry[spotValue].GetComponent<Unit>().def.ToString();
        }
    }
}
