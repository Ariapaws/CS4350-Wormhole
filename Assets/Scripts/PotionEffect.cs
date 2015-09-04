using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PotionEffect : MonoBehaviour {

	public int healthIncrease = 20;
	GameObject player;
	PlayerHealth playerHealth;
	bool potionUsed = false;
	public Text feedback;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerHealth> ();
		GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
		feedback = feedbackObject.GetComponent<Text>();
	}
	
	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject == player && !potionUsed)
		{
			playerHealth.UsePotion(healthIncrease);
			potionUsed = true;
			feedback.color = new Color(1,1,1,2);
			feedback.text = "You have healed "+healthIncrease.ToString()+" health.";
			Destroy (gameObject, 0f);
		}
	}
	
	
	void OnTriggerExit (Collider other)
	{

	}
}
