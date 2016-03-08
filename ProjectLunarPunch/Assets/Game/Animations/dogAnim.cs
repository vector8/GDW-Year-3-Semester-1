using UnityEngine;
using System.Collections;

public class dogAnim : MonoBehaviour {

    public Animator anim;

	// Use this for initialization
	void Start () {
        anim.GetComponent<Animator>();
	}
	
	public void setRun(bool poop)
    {
        anim.SetBool("run", poop);
    }


}
