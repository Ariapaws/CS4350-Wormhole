using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	public GameObject target;
	public string[] attacks;
	public float range = 5f;
	public int damagePerHit = 20;
	public AudioClip hitClip;
	public AudioClip swingClip;
	public bool attackPerforming = false;

	int attacknumber = 0;
	int shootableMask;
	Ray shootRay;
	RaycastHit shootHit;
	AudioSource playerAudio;

	// For spherecast (torch)
	float sphereThickness = 0.4f;
	float sphereRange = 2.3f;
	Vector3 sphereOrigin;
	Vector3 sphereDirection;
	RaycastHit sphereHit;

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
			attackPerforming = true;
			target.GetComponent<Animation>().Play (attacks[attacknumber]);
			attacknumber++;
			if (attacknumber == 3) {
				attacknumber = 0;
			}
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
