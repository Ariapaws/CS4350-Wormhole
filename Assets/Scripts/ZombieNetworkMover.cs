using UnityEngine;
using System.Collections;

public class ZombieNetworkMover : Photon.MonoBehaviour {
	private Vector3 correctZombiePosition;
	private Quaternion correctZombieRotation;
	// Use this for initialization
	void Start () {
		if (!PhotonNetwork.isMasterClient) {
			StartCoroutine(UpdateData());
		}
	}
	IEnumerator UpdateData() {
		while(true)
		{
			
			if (!photonView.isMine) {
				transform.position = Vector3.Lerp(transform.position, correctZombiePosition, Time.deltaTime * 5);
				transform.rotation = Quaternion.Lerp (transform.rotation, correctZombieRotation, Time.deltaTime * 5);
				
			}
			yield return null;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		Animator anim = transform.GetComponentInChildren<Animator> ();
		
		if (stream.isWriting) {
			stream.SendNext(transform.position);	
			stream.SendNext(transform.rotation);
		}  else {
			this.correctZombiePosition = (Vector3) stream.ReceiveNext();
			this.correctZombieRotation =  (Quaternion) stream.ReceiveNext();
		}
	}
}
