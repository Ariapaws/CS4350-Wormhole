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
	CustomTeleporter teleportBaseScript;
	public AudioClip cashClip;
	public AudioClip errorClip;
	AudioSource playerAudio;
	public GameObject BaseTeleport;

	// Use this for initialization
	void Awake () {
		currentCash = startingCash;
		playerAudio = GetComponent <AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F)) {
			tryToPlaceTorch();
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

	public void tryToPlaceTorch(){
		Vector3 torchPosition = transform.position + transform.forward * 2.0f;
		if (GetClosestTorchDistance(torchPosition) > 3.0f) {
			Instantiate(torchInstance, torchPosition, Quaternion.identity);
		}
		else {
			// NEED TO SAVE THE CLIP THAT WAS IN playerAudio before that anot? :(
			//AudioClip temp = playerAudio.clip;
			playerAudio.clip = errorClip;
			playerAudio.Play();
		}
	}

	public void placeTeleport(){
		Vector3 teleportPosition = transform.position + transform.forward * 2.0f;
		
		if (teleportA != null) {
			Destroy(teleportA);
		}
		teleportA = (GameObject)Instantiate (teleportInstance, teleportPosition, Quaternion.identity);
		teleportAScript = teleportA.GetComponentInChildren<CustomTeleporter>();
		teleportBaseScript = BaseTeleport.GetComponentInChildren<CustomTeleporter>();
		teleportAScript.destinationPad.SetValue (teleportBaseScript.gameObject.transform, 0);
		teleportAScript.teleportPadOn = true;
		teleportBaseScript.destinationPad.SetValue (teleportAScript.gameObject.transform, 0);
		teleportBaseScript.teleportPadOn = true;
		
		
	}
	private float GetClosestTorchDistance(Vector3 torchPos)
	{
		GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Torch");
		GameObject closestObject = null;
		float closestDistance = float.MaxValue;
		foreach (GameObject obj in objectsWithTag)
		{
			if(!closestObject)
			{
				closestObject = obj;
				closestDistance = Vector3.Distance(torchPos, obj.transform.position);
			}
			//compares distances
			if(Vector3.Distance(torchPos, obj.transform.position) <= Vector3.Distance(torchPos, closestObject.transform.position))
			{
				closestObject = obj;
				closestDistance = Vector3.Distance(torchPos, obj.transform.position);
			}
		}
		return closestDistance;
	}



		/*else{
			Destroy (teleportA);
			teleportA = (GameObject)Instantiate (teleportInstance, teleportPosition, Quaternion.identity);
			teleportAScript = teleportA.GetComponentInChildren<CustomTeleporter>();
			teleportBScript = teleportB.GetComponentInChildren<CustomTeleporter>();
			teleportAScript.destinationPad.SetValue(teleportBScript.gameObject.transform, 0); 
			teleportAScript.teleportPadOn = true;
			teleportBScript.destinationPad.SetValue(teleportAScript.gameObject.transform, 0); 
			teleportBScript.teleportPadOn = true;
		} */

	


}
