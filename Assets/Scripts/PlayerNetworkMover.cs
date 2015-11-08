using UnityEngine;
using System.Collections;

public class PlayerNetworkMover : Photon.MonoBehaviour {
	private Vector3 correctPlayerPosition;
	private Quaternion correctPlayerRotation;
	public Animator anim;
	// Use this for initialization
	
	void Start () {
//		foreach (Transform child in transform) {
//			if (child.gameObject.name=="Vanille") {
//				anim = child.gameObject.GetComponent<Animator>();
//			}
//		}

		if (anim == null) {
			Debug.LogError ("no anim");
		} else {
			Debug.Log(anim.name);
		}

		if (photonView.isMine) {
			GameObject.FindGameObjectWithTag("Samuzai").GetComponent<Shop>().enabled = true;
			GameObject.FindGameObjectWithTag("Samuzai").GetComponent<Shop>().player = this.gameObject;

			GameObject.FindGameObjectWithTag("NotDestroyed").GetComponent<Score>().enabled = true;
			GameObject.FindGameObjectWithTag("NotDestroyed").GetComponent<Score>().assets = GetComponent<PlayerAssets>();

			GameObject miniMapCamera = GameObject.FindGameObjectWithTag("MiniMapCamera");
			miniMapCamera.GetComponent<Camera>().enabled = true;
			miniMapCamera.GetComponent<CameraFollow>().enabled = true;
			miniMapCamera.GetComponent<CameraFollow>().target = this.gameObject;

			GameObject miniMapReveal = GameObject.FindGameObjectWithTag("MiniMapReveal");
			miniMapReveal.GetComponent<MinimapReveal>().enabled = true;
			miniMapReveal.GetComponent<MinimapReveal>().player = this.gameObject;


			//ENABLE PLAYER HEALTH SCRIPT
			GameObject HUDCanvas = GameObject.FindGameObjectWithTag("HUDCanvas");
			GameObject healthSlider = GameObject.FindGameObjectWithTag("HealthSlider");
			GameObject staminaSlider = GameObject.FindGameObjectWithTag("StaminaSlider");
			GameObject feedback = GameObject.FindGameObjectWithTag("Feedback");
			GameObject damageImage = GameObject.FindGameObjectWithTag("DamageImage");
			GetComponent<PlayerHealth>().enabled = true;
			GetComponent<PlayerHealth>().healthSlider = healthSlider.GetComponent<UnityEngine.UI.Slider>();
			GetComponent<PlayerHealth>().staminaSlider = staminaSlider.GetComponent<UnityEngine.UI.Slider>();
			GetComponent<PlayerHealth>().feedback = feedback.GetComponent<UnityEngine.UI.Text>();
			GetComponent<PlayerHealth>().damageImage = damageImage.GetComponent<UnityEngine.UI.Image>();

			//ENABLE PLAYER ASSETS SCRIPT
			GameObject cashUI = GameObject.FindGameObjectWithTag("CashUI");
			GameObject baseTeleport = GameObject.FindGameObjectWithTag("BaseTeleport");
			GameObject potionUI = GameObject.FindGameObjectWithTag("potion-number");
			GetComponent<PlayerAssets>().enabled = true;
			GetComponent<PlayerAssets>().cashAmountDisplay = cashUI.GetComponent<UnityEngine.UI.Text>();
			GetComponent<PlayerAssets>().BaseTeleport = baseTeleport;
			GetComponent<PlayerAssets>().potionUI = potionUI;
			GetComponent<PlayerAssets>().feedback = feedback.GetComponent<UnityEngine.UI.Text>();

			GameObject shopUI = GameObject.FindGameObjectWithTag("Shop");
			GetComponent<PlayerPurchaseUI>().enabled = true;
			GetComponent<PlayerPurchaseUI>().canvas = shopUI;
			GetComponent<PlayerPurchaseUI>().feedback = feedback.GetComponent<UnityEngine.UI.Text>();

			HUDCanvas.GetComponent<Canvas>().enabled = true;

			GetComponent<CharacterController> ().enabled = true;
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = true;
			GetComponent<PlayerAttack> ().enabled = true;
			GetComponentInChildren<macheteAnimatorControllerScript> ().enabled = true;
			foreach (Camera cam in GetComponentsInChildren<Camera>()) {
				cam.enabled = true;
//				cam.cullingMask = (1<<0 | 1<<10);
			}
			GetComponentInChildren<AudioListener>().enabled = true;

            GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i=0; i<zombies.Length; i++)
            {
                zombies[i].GetComponent<ZombieHealth>().enabled = true;
                zombies[i].GetComponent<ZombieAttack>().player = this.gameObject;
                zombies[i].GetComponent<ZombieAttack>().enabled = true;
                zombies[i].GetComponent<ZombieAttack>().zombieHealth = zombies[i].GetComponent<ZombieHealth>();
                zombies[i].GetComponent<ZombieAttack>().anim = zombies[i].GetComponent<Animator>();
                zombies[i].GetComponent<ZombieAttack>().zombieAudio = zombies[i].GetComponent<AudioSource>();
                if (zombies[i].GetComponent<ZombieAttack>().feedback == null) {
                    GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
                    zombies[i].GetComponent<ZombieAttack>().feedback = feedbackObject.GetComponent<UnityEngine.UI.Text>();
                }
            }

            GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
            gm.GetComponent<GameCountDown>().player = this.gameObject;
        }  
		else {
			foreach (Transform child in transform) {
				if (child.gameObject.name=="Vanille") {
					ChangeLayersRecursively(child, "Default");
				}
			}
			
			foreach (Transform child in transform) {
				if (child.gameObject.name == "Machete_FPS") {
					ChangeLayersRecursively(child, "3rdPersonModel");
				}
			}
			
			StartCoroutine("Update Data");
		}
	}
	
	IEnumerator UpdateData() {
		while(true)
		{
			
			if (!photonView.isMine) {
				transform.position = Vector3.Lerp(transform.position, correctPlayerPosition, Time.deltaTime * 5);
				transform.rotation = Quaternion.Lerp (transform.rotation, correctPlayerRotation, Time.deltaTime * 5);
				
			}
			yield return null;
		}
	}
	
	//	void Update() {
	//		if (!photonView.isMine) {
	//			transform.position = Vector3.Lerp(transform.position, correctPlayerPosition, 0.1f);
	//			transform.rotation = Quaternion.Lerp (transform.rotation, correctPlayerRotation, 0.1f);
	//		}
	//	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		Animator anim = transform.GetComponentInChildren<Animator> ();
		
		if (stream.isWriting) {
			stream.SendNext(transform.position);	
			stream.SendNext(transform.rotation);
			stream.SendNext(anim.GetFloat("Forward"));
			stream.SendNext(anim.GetFloat("Turn"));
			stream.SendNext(anim.GetBool("OnGround"));
			stream.SendNext(anim.GetFloat("Jump"));
			stream.SendNext(anim.GetFloat("JumpLeg"));
			stream.SendNext(anim.GetBool("Guard"));
			stream.SendNext(anim.GetFloat("Block"));
		}  else {
			this.correctPlayerPosition = (Vector3) stream.ReceiveNext();
			this.correctPlayerRotation =  (Quaternion) stream.ReceiveNext();
			this.anim.SetFloat("Forward", (float) stream.ReceiveNext());
			this.anim.SetFloat("Turn", (float) stream.ReceiveNext());
			this.anim.SetBool("OnGround", (bool) stream.ReceiveNext());
			this.anim.SetFloat("Jump", (float) stream.ReceiveNext());
			this.anim.SetFloat("JumpLeg", (float) stream.ReceiveNext());
			this.anim.SetBool("Guard", (bool) stream.ReceiveNext());
			this.anim.SetFloat("Block", (float) stream.ReceiveNext());
		}
	}

	void ChangeLayersRecursively(Transform trans, string name)
	{
		foreach (Transform child in trans)
		{
			child.gameObject.layer = LayerMask.NameToLayer(name);
			ChangeLayersRecursively(child, name);
		}
	}

	[PunRPC]
	void SetTrigger(string triggerName) {
		Debug.Log ("TRIGGERED " + triggerName); 
		anim.SetTrigger (triggerName);
	}
}

