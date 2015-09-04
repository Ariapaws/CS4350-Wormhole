using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CrateEffect : MonoBehaviour {

	public int cashAmount = 20;
	GameObject player;
	PlayerAssets PlayerAssets;
	bool carteCollected = false;

	public Text feedback;
	
	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		PlayerAssets = player.GetComponent <PlayerAssets> ();
		GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
		feedback = feedbackObject.GetComponent<Text>();
	}
	
	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject == player && !carteCollected)
		{
			PlayerAssets.AddCash(cashAmount);
			carteCollected = false;

			feedback.color = new Color(1,1,1,2);
			feedback.text = "You have picked up "+cashAmount.ToString()+" points.";
			Destroy (gameObject, 0f);
			/*
			TimeSpan ts = DateTime.Now + TimeSpan.FromSeconds (2);
			do {
				feedback.text = "You have picked up "+cashAmount.ToString()+" points.";
			} while (DateTime.Now <ts);
			feedback.text = "";
			*/

		}
	}
	
	
	void OnTriggerExit (Collider other)
	{
		
	}
}
