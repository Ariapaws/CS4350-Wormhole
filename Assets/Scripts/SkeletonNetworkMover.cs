using UnityEngine;
using System.Collections;

public class SkeletonNetworkMover : MonoBehaviour {
	private Vector3 correctSkeletonPosition;
	private Quaternion correctSkeletonRotation;
	// Use this for initialization
	void Start () {
		if (!PhotonNetwork.isMasterClient) {
			StartCoroutine (UpdateData ());
		} 
	}
	IEnumerator UpdateData() {
		while(true)
		{
			if (!PhotonNetwork.isMasterClient) {
				transform.position = Vector3.Lerp(transform.position, correctSkeletonPosition, Time.deltaTime * 5);
				transform.rotation = Quaternion.Lerp (transform.rotation, correctSkeletonRotation, Time.deltaTime * 5);
				
			}
			yield return null;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext(transform.position);	
			stream.SendNext(transform.rotation);
			
		}  else {
			this.correctSkeletonPosition = (Vector3) stream.ReceiveNext();
			this.correctSkeletonRotation =  (Quaternion) stream.ReceiveNext();
		}
	}
}
