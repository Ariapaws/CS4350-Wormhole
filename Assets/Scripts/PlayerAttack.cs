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
	
	// defense
	public int defense = 5;


/**************** End of variables to be changed ********************************/

	public GameObject target;

	int attacknumber = 0;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	AudioSource playerAudio;
	public Animator animator;
	public AudioClip hitClip;
	public AudioClip swingClip;
	public bool attackPerforming = false;

	// For spherecast (torch)
	float sphereThickness = 0.4f;
	float sphereRange = 2.3f;
	Vector3 sphereOrigin;
	Vector3 sphereDirection;
	RaycastHit sphereHit;

	// Use this for initialization
	void Start () {
		playerAudio = GetComponent <AudioSource> ();
		shootableMask = LayerMask.GetMask ("Shootable");
		animator = GetComponentInChildren<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (1)) {
			//block animation enabled
			animator.SetBool ("Guard", true);
			GetComponent<PlayerHealth>().armor += defense;

		} else if (Input.GetMouseButtonUp (1)) {
			//block animation disabled
			animator.SetBool("Guard", false);
			GetComponent<PlayerHealth>().armor -= defense;
		}
			
		else if (Input.GetAxis ("Fire1")>0) {

			// attack animation enabled
			shootRay.origin = transform.position;
			shootRay.direction = transform.forward;

			// For spherecast (torch)
			sphereOrigin = transform.position;
			sphereDirection = transform.TransformDirection(Vector3.forward);
			StartCoroutine(attack());

		}
	}

	IEnumerator  attack() {

		if (attackPerforming) {
			yield return new WaitForSeconds(0);
		} else {
			animator.SetTrigger("Attack");

			attackPerforming = true;
			bool isSphereCastHit = Physics.SphereCast(sphereOrigin, sphereThickness, sphereDirection, out sphereHit, sphereRange);
			if(Physics.Raycast (shootRay, out shootHit, range))
			{
				Debug.DrawLine(shootRay.origin, shootHit.point);
				ZombieHealth enemyHealth = shootHit.collider.GetComponent <ZombieHealth> ();
				if(enemyHealth != null)
				{
					enemyHealth.TakeDamage (damagePerHit);
					playerAudio.clip = hitClip;
					playerAudio.Play();
				} else if (isSphereCastHit){
					tryToHitTorch(sphereHit);
				} else {
					playerAudio.clip = swingClip;
					playerAudio.Play();
				}
			} else if (isSphereCastHit){
				tryToHitTorch(sphereHit);
			} else {
				playerAudio.clip = swingClip;
				playerAudio.Play();
			}
			yield return new WaitForSeconds(0.5f);
			attackPerforming = false;
		}
	}

	void tryToHitTorch(RaycastHit sphereHit){
		GameObject torchObject = null;
		Torch torchScript = sphereHit.collider.GetComponent<Torch>();
		if(torchScript != null){
			torchObject = torchScript.gameObject;
		}
		if (torchObject != null) {
			playerAudio.clip = hitClip;
			playerAudio.Play ();
			Destroy (torchObject);
			PlayerAssets playerAssetsScript = GetComponent<PlayerAssets> ();
			playerAssetsScript.numOfTorchesLeft++;
		} else {
			playerAudio.clip = swingClip;
			playerAudio.Play ();
		}
	}
}
