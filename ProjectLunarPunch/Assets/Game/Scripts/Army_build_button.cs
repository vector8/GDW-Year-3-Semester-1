using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Army_build_button : MonoBehaviour {
    private SplineWalker walker=GameObject.Find("Main Camera").GetComponent<SplineWalker>();
    private Button pressedButton = null;

    //walker.spline = GameObject.Find("Main -> Army Spline").GetComponent<BezierSpline>();
	

	// Use this for initialization
	void Start () {
       }
	
	// Update is called once per frame
	void Update () 
    {
        if (pressedButton != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(MouseInputWrapper.getMousePosition());

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Army_Flag" && MouseInputWrapper.getMouseButtonDown(MouseInputWrapper.MouseButtons.Left))
                {
                    walker.enabled=true;
                }
            }
        }

        if (walker.progress == 1f)
        {
            Application.LoadLevel("ArmyBuilder2");
        }
	
	}

    //script for clicking on flag
    /*void OnMouseDown()
    {
        walker.spline = GameObject.Find("Main -> Army Spline").GetComponent<BezierSpline>();
        walker.enabled = true;
    }*/
}
