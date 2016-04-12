using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class YourIP : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Text>().text = "Your IP: " + NetworkWrapper.getLocalIPAddress();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
