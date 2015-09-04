using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAssets : MonoBehaviour {

	public int startingCash = 0;
	public int currentCash;
	public Text cashAmountDisplay;
	public GameObject torchInstance;
	public GameObject teleportInstance;
	public GameObject teleportA;
	CustomTeleporter teleportAScript;
	CustomTeleporter teleportBScript;
	public GameObject teleportB;
	public AudioClip cashClip;
	AudioSource playerAudio;

	// Use this for initialization
	void Awake () {
		currentCash = startingCash;
		playerAudio = GetComponent <AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			placeTorch();
		}
		if (Input.GetKeyDown(KeyCode.E)){
			placeTeleport();
		}
	}

	public void AddCash (int amount)
	{
		
		currentCash += amount;

		cashAmountDisplay.text = currentCash.ToString ();
		playerAudio.clip = cashClip;
		playerAudio.Play ();
		
		
	}

	public void placeTorch(){
		Vector3 torchPosition = transform.position + transform.forward * 2.0f;
		Instantiate(torchInstance, torchPosition, Quaternion.identity);
	}

	public void placeTeleport(){
		Vector3 teleportPosition = transform.position + transform.forward * 2.0f;

		if (teleportA == null) {
			teleportA = (GameObject)Instantiate (teleportInstance, teleportPosition, Quaternion.identity);
			teleportAScript = teleportA.GetComponentInChildren<CustomTeleporter>();
			teleportAScript.teleportPadOn = false;
		} else if(teleportB != null){
			Destroy (teleportA);
			teleportA = teleportB;
			teleportB = (GameObject)Instantiate (teleportInstance, teleportPosition, Quaternion.identity);
			teleportAScript = teleportA.GetComponentInChildren<CustomTeleporter>();
			teleportBScript = teleportB.GetComponentInChildren<CustomTeleporter>();
			teleportAScript.destinationPad.SetValue(teleportBScript.gameObject.transform, 0); 
			teleportAScript.teleportPadOn = true;
			teleportBScript.destinationPad.SetValue(teleportAScript.gameObject.transform, 0); 
			teleportBScript.teleportPadOn = true;
		} else {
			teleportB = (GameObject)Instantiate (teleportInstance, teleportPosition, Quaternion.identity);
			teleportAScript = teleportA.GetComponentInChildren<CustomTeleporter>();
			teleportBScript = teleportB.GetComponentInChildren<CustomTeleporter>();
			teleportAScript.destinationPad.SetValue(teleportBScript.gameObject.transform, 0); 
			teleportAScript.teleportPadOn = true;
			teleportBScript.destinationPad.SetValue(teleportAScript.gameObject.transform, 0); 
			teleportBScript.teleportPadOn = true;
		}

	}


}
