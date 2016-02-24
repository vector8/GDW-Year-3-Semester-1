using UnityEngine;
using System.Collections;

public class dogAnim : MonoBehaviour {

    public Animator anim;

	// Use this for initialization
	void Start () {
        anim.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("run");
            anim.SetBool("isRun", true);
        }
        if(Input.GetMouseButtonDown(1))
        {
            anim.SetBool("isRun", false);
        }
	
	}
}
