using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	public GameObject player;
	public PlayerAssets PlayerAssets;
	public Text feedback;
	
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent <Animator> ();
		zombieAudio = GetComponent <AudioSource> ();
		//hitParticles = GetComponentInChildren <ParticleSystem> ();
		capsuleCollider = GetComponent <CapsuleCollider> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		PlayerAssets = player.GetComponent <PlayerAssets> ();
		currentHealth = startingHealth;
		GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
		feedback = feedbackObject.GetComponent<Text>();
	}
	
	
	void Update ()
	{
		if(isSinking)
		{
			transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		}
	}
	

	[PunRPC]
	void TakeDamage (int amount)
	{
		Debug.Log ("Taking Damage OUCHY ouch ouch! " + currentHealth);
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
			this.gameObject.GetComponent<PhotonView>().RPC("Death", PhotonTargets.All);
			Death();
		}
	}
	
	[PunRPC]
	void Death ()
	{
		Debug.Log ("DEAD");
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
		feedback.color = new Color(1,1,1,2);
		if (random<0.50f){
			PlayerAssets.AddCash(cashAmount+20);
			feedback.text = "You have gained "+(cashAmount+20).ToString()+" points.";
			//Instantiate(crateInstance,dropOffPos, Quaternion.identity);
		} else if (random<0.75f){
			PlayerAssets.AddCash(cashAmount);
			feedback.text = "You have gained "+cashAmount.ToString()+" points.";
			Instantiate(potionInstance,dropOffPos, Quaternion.identity);
		} else {
			PlayerAssets.AddCash(cashAmount);
			feedback.text = "You have gained "+cashAmount.ToString()+" points.";
			//Instantiate(clockInstance,dropOffPos, Quaternion.identity);
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
		GameObject.Destroy (this.gameObject);
		PhotonNetwork.Destroy (this.gameObject);
	}
}


