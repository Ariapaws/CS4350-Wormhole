using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

/**************** Start of variables to be changed ********************************/

	//attack range
	public float range = 5f;

	// attack speed (for animation...not sure does it really kill faster)
	public float attackSpeed = 1f;

	// base damage
	public int damagePerHit = 20;

/**************** End of variables to be changed ********************************/

	public GameObject target;
	public string[] attacks;
	int attacknumber = 0;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	AudioSource playerAudio;
	public AudioClip hitClip;
	public AudioClip swingClip;
	public bool attackPerforming = false;
	// Use this for initialization
	void Start () {
		playerAudio = GetComponent <AudioSource> ();
		attacks = new string[3];
		attacks[0] = "attack1";
		attacks[1] = "attack2";
		attacks[2] = "attack3";
		shootableMask = LayerMask.GetMask ("Shootable");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Fire1")>0) {
			StartCoroutine(attack());
			shootRay.origin = transform.position;
			shootRay.direction = transform.forward;

		}
	}

	IEnumerator  attack() {

		if (attackPerforming) {
			yield return new WaitForSeconds(0);
		} else {
			attackPerforming = true;
			target.GetComponent<Animation>().Play (attacks[attacknumber]);
			target.GetComponent<Animation>()[attacks[attacknumber]].speed = attackSpeed;
			attacknumber++;
			if (attacknumber == 3) {
				attacknumber = 0;
			}
			if(Physics.Raycast (shootRay, out shootHit, range))
			{
				Debug.DrawLine(shootRay.origin, shootHit.point);
				ZombieHealth enemyHealth = shootHit.collider.GetComponent <ZombieHealth> ();
				if(enemyHealth != null)
				{
					enemyHealth.TakeDamage (damagePerHit);
					playerAudio.clip = hitClip;
					playerAudio.Play();
				} else {
					playerAudio.clip = swingClip;
					playerAudio.Play();
				}
				
			} else {
				playerAudio.clip = swingClip;
				playerAudio.Play();
			}
			yield return new WaitForSeconds(0.5f);
			target.GetComponent<Animation>().Play ("idle");
			attackPerforming = false;
		}
	}


	IEnumerator  powerattack() {
		target.GetComponent<Animation>().Play ("powerAttack");
		yield return new WaitForSeconds(1.7f);
		target.GetComponent<Animation>().Play ("idle");
	}

	IEnumerator  finishattack() {
		target.GetComponent<Animation>().Play ("finishAttack");
		yield return new WaitForSeconds(1.5f);
		target.GetComponent<Animation>().Play ("idle");
	}
}
