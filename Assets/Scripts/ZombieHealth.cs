using UnityEngine;
using System.Collections;

public class ZombieHealth : MonoBehaviour {

	public int startingHealth = 100;
	public int currentHealth;
	public int cashAmount = 20;
	public float sinkSpeed = 2.5f;
	public int scoreValue = 10;
	public AudioClip deathClip;
	public Rigidbody rb;
	
	
	Animator anim;
	AudioSource zombieAudio;
	//ParticleSystem hitParticles;
	CapsuleCollider capsuleCollider;
	public bool isDead;
	bool isSinking;
	public GameObject crateInstance;
	public GameObject potionInstance;
	public GameObject clockInstance;
	GameObject player;
	PlayerAssets PlayerAssets;
	
	void Awake ()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent <Animator> ();
		zombieAudio = GetComponent <AudioSource> ();
		//hitParticles = GetComponentInChildren <ParticleSystem> ();
		capsuleCollider = GetComponent <CapsuleCollider> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		PlayerAssets = player.GetComponent <PlayerAssets> ();
		currentHealth = startingHealth;
	}
	
	
	void Update ()
	{
		if(isSinking)
		{
			transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		}
	}
	
	
	public void TakeDamage (int amount)
	{
		if(isDead)
			return;
		
		//enemyAudio.Play ();
		
		currentHealth -= amount;
		anim.SetTrigger ("GetHit");
		//rb.AddForce(-transform.forward * 10, ForceMode.VelocityChange);
		//rb.AddForce(transform.forward * 10, ForceMode.Impulse);
		
		//hitParticles.transform.position = hitPoint;
		//hitParticles.Play();
		
		if(currentHealth <= 0)
		{
			Death ();
		}
	}
	
	
	void Death ()
	{
		isDead = true;

		if (anim) {
			anim.SetTrigger ("Dead");
		} else {
			GetComponent<Animation>().Play ("death",PlayMode.StopSameLayer);
		}
		zombieAudio.clip = deathClip;
		zombieAudio.Play ();
		//capsuleCollider.height = 0.4f;

		Vector3 dropOffPos = new Vector3(transform.position.x, 1.0f, transform.position.z);
		float random = Random.Range(0f,1f);
		PlayerAssets.AddCash(cashAmount);
		if (random<0.50f){
			Instantiate(crateInstance,dropOffPos, Quaternion.identity);
		} else if (random<0.75f){
			Instantiate(potionInstance,dropOffPos, Quaternion.identity);
		} else {
			Instantiate(clockInstance,dropOffPos, Quaternion.identity);
		}
		StartCoroutine(StartSinking ());

	}
	
	
	public IEnumerator StartSinking ()
	{
		yield return new WaitForSeconds(0f);
		capsuleCollider.isTrigger = true;
		GetComponent <Rigidbody> ().isKinematic = true;
		GetComponent <Rigidbody> ().useGravity = true;
		//GetComponent <NavMeshAgent> ().enabled = false;

		isSinking = true;
		//ScoreManager.score += scoreValue;
		Destroy (gameObject, 5f);
	}
}


