using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Stats_UI_Handler : MonoBehaviour {

    public GameObject statsPlane;

    public GameObject arrayhandle;

    public GameObject[] arry = new GameObject[5];

    public Canvas localHandle;

    public GameObject spot;

    int spotValue;


	// Use this for initialization
	void Start () {

       arry = arrayhandle.GetComponent<SoldierButton>().g;
       spotValue = int.Parse(spot.name);
	}
	
	// Update is called once per frame
	void Update () {
       // activeBut = ((buttonLocations)arrayhandle.GetComponent<SoldierButton>().ID);


        if(arry[spotValue-1] != null)
        {
            localHandle.GetComponentsInChildren<Text>()[0].text = arry[spotValue - 1].GetComponent<Unit>().hp.ToString();
            localHandle.GetComponentsInChildren<Text>()[1].text = arry[spotValue - 1].GetComponent<Unit>().speed.ToString();
            localHandle.GetComponentsInChildren<Text>()[2].text = arry[spotValue - 1].GetComponent<Unit>().att.ToString();
            localHandle.GetComponentsInChildren<Text>()[3].text = arry[spotValue - 1].GetComponent<Unit>().def.ToString();
        }

	}
}
