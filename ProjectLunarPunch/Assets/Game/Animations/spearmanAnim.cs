using UnityEngine;
using System.Collections;

public class spearmanAnim : MonoBehaviour {

    public Animator anim;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	
    public void setRun(bool param)
    {
        anim.SetBool("isRun", param);
    }

    public void setDeath(bool param)
    {
        anim.SetBool("isDeath", param);
    }

    public void callHit()
    {
        anim.SetTrigger("hit");
    }

    public void callAttack()
    {
        anim.SetTrigger("attack");
    }

}
