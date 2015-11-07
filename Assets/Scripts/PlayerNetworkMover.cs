using UnityEngine;
using System.Collections;

public class PlayerNetworkMover : Photon.MonoBehaviour {
	private Vector3 correctPlayerPosition;
	private Quaternion correctPlayerRotation;
	private Animator anim;
	// Use this for initialization
	
	void Start () {
		anim = transform.GetComponentInChildren<Animator> ();
		
		if (photonView.isMine) {
			GetComponent<CharacterController> ().enabled = true;
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = true;
			GetComponent<PlayerAttack> ().enabled = true;
			GetComponentInChildren<macheteAnimatorControllerScript> ().enabled = true;
			foreach (Camera cam in GetComponentsInChildren<Camera>()) {
				cam.enabled = true;
//				cam.cullingMask = (1<<0 | 1<<10);
			}
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
			//			stream.SendNext(anim.GetFloat("Forward"));
			//			stream.SendNext(anim.GetFloat("Turn"));
			//			stream.SendNext(anim.GetBool("OnGround"));
			//			stream.SendNext(anim.GetFloat("Jump"));
		}  else {
			this.correctPlayerPosition = (Vector3) stream.ReceiveNext();
			this.correctPlayerRotation =  (Quaternion) stream.ReceiveNext();
//			anim.SetFloat("Forward", (float) stream.ReceiveNext());
//			anim.SetFloat("Turn", (float) stream.ReceiveNext());
//			anim.SetBool("OnGround", (bool) stream.ReceiveNext());
//			anim.SetFloat("Jump", (float) stream.ReceiveNext());
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
	
}

