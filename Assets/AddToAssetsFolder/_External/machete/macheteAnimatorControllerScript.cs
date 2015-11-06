using UnityEngine;
using System.Collections;

public class macheteAnimatorControllerScript : MonoBehaviour {

    public Animator machete_anim;
    public string [] attackName;

	// Use this for initialization
	void Start () {
        machete_anim = GetComponent<Animator>();
        attackName = new string [] {"Attack", "Attack1", "Attack2"};
	}
	
	// Update is called once per frame
	void Update () {
	     if (Input.GetMouseButtonDown(1) && Input.GetAxis("Fire1") <= 0)
        {
            if (machete_anim.GetInteger("Block") == 0)
            {
                machete_anim.SetInteger("Block", 1);
            }

		} else if (Input.GetMouseButtonUp (1)) {
            machete_anim.SetInteger("Block", 0);
           
		}
		
	    // if attacking without blocking
         else if (Input.GetAxis("Fire1") > 0 && !Input.GetMouseButtonDown(1))
         {
             machete_anim.SetInteger("Block", 0);
             machete_anim.SetTrigger(attackName[Random.Range(0,3)]);
         }
         // if attacking and blocking
         else if (Input.GetAxis("Fire1") > 0 && Input.GetMouseButtonDown(1))
         {
             //block animation enabled
             machete_anim.SetInteger("Block", 1);
         }
    }
}
