using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZombieAttack : MonoBehaviour {

	public float timeBetweenAttacks = 0.5f;
	public int attackDamage = 10;
	public AudioClip attackClip;
	
	Animator anim;
	GameObject player;
	PlayerHealth playerHealth;
	AudioSource zombieAudio;
	ZombieHealth zombieHealth;
	public bool playerInRange;
	public float timer;
	public bool isAttacking;

	public Text feedback;
	
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerHealth> ();
		zombieAudio = GetComponent <AudioSource> ();
		zombieHealth = GetComponent<ZombieHealth>();
		anim = GetComponent <Animator> ();
		GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
		feedback = feedbackObject.GetComponent<Text>();
	}
	
	
	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject == player)
		{
			playerInRange = true;

		}
	}

	void OnTriggerStay(Collider other){
		if(other.gameObject == player)
		{
			playerInRange = true;
			
		}
	}

	void OnTriggerExit (Collider other)
	{
		if(other.gameObject == player)
		{
			playerInRange = false;
			
		}
	}
	
	
	void Update ()
	{
		timer += Time.deltaTime;
		if(timer >= timeBetweenAttacks && playerInRange && zombieHealth.currentHealth > 0)
		{
			if(anim){
				anim.SetTrigger ("Attack");
			} else {
				GetComponent<Animation>().Play ("punch",PlayMode.StopSameLayer);
			}
			Attack ();
		}
		/*
		
		if(playerHealth.currentHealth <= 0)
		{
			anim.SetTrigger ("PlayerDead");
		}*/
	}

	bool attackComplete(){

		Debug.Log ((anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit")));
	
		if (this.isAttacking && !(anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit")))
		{
			this.isAttacking = false;
			Debug.Log("attack completed");
			return true;
			// You have just leaved your state!
		} else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
		{
			// Avoid any reload.
			this.isAttacking = true;
			Debug.Log("attack in progress");
			return false;
			
		} else {
			this.isAttacking = false;
			Debug.Log("attack interrputed");
			return false;
		}
	}
	
	
	void Attack ()
	{


		if(playerHealth.currentHealth > 0 && attackComplete())
		{
			timer = 0f;
			zombieAudio.clip = attackClip;
			zombieAudio.Play ();
			Vector3 direction = (new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position).normalized;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, 2f, 0.0F);
			transform.rotation = Quaternion.LookRotation(newDir);
			if (playerHealth.armor >= attackDamage) {
				feedback.color = new Color(102,178,255,1);
				feedback.text = "Damage blocked";
			}
			else {
				feedback.color = new Color(1,0,0,2);
				feedback.text = "Damage received: "+attackDamage.ToString();
				playerHealth.TakeDamage (attackDamage);
			}
		}

	}
}
