using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public int startingHealth = 300;
	public int currentHealth;
	public Slider healthSlider;
	public Image damageImage;
	//public AudioClip deathClip;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	public Text feedback;
	public AudioClip teleportClip;
	public AudioClip potionClip;
	AudioSource playerAudio;
	
	//Animator anim;
	//AudioSource playerAudio;
	//PlayerMovement playerMovement;
	//PlayerShooting playerShooting;
	bool isDead;
	bool damaged;
	
	
	void Awake ()
	{
		//anim = GetComponent <Animator> ();
		//playerAudio = GetComponent <AudioSource> ();
		//playerMovement = GetComponent <PlayerMovement> ();
		//playerShooting = GetComponentInChildren <PlayerShooting> ();
		currentHealth = startingHealth;
		playerAudio = GetComponent <AudioSource> ();
		GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
		feedback = feedbackObject.GetComponent<Text>();
	}
	
	
	void Update ()
	{
		if(damaged)
		{
			damageImage.color = flashColour;
		}
		else
		{
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		damaged = false;
		if (transform.position.y < 0) {
			transform.position = new Vector3(transform.position.x, 6.0f, transform.position.z);
		}
	}
	
	
	public void TakeDamage (int amount)
	{
		damaged = true;
		
		currentHealth -= amount;
		
		healthSlider.value = currentHealth;
		
		//playerAudio.Play ();
		
		if(currentHealth <= 0 && !isDead)
		{
			Death ();
		}
	}

	public void UsePotion (int amount)
	{

		currentHealth += amount;

		if(currentHealth >= 300)
		{
			currentHealth = 300;
		}
		
		healthSlider.value = currentHealth;
		playerAudio.clip = potionClip;
		playerAudio.Play ();
		

	}
	
	
	void Death ()
	{
		isDead = true;
		
		//playerShooting.DisableEffects ();
		
		//anim.SetTrigger ("Die");
		
		//playerAudio.clip = deathClip;
		//playerAudio.Play ();
		
		//playerMovement.enabled = false;
		//playerShooting.enabled = false;

		Vector3 temp = new Vector3(-14.6f, 1.8f, -21.5f);
		playerAudio.clip = teleportClip;
		playerAudio.Play();
		transform.position = temp;
		currentHealth = startingHealth;
		healthSlider.value = currentHealth;
		isDead = false;
		feedback.color = new Color(124,168,255,255);
		feedback.text = "You have died and have been brought back to base.";


	}
	
	
	public void RestartLevel ()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
}
