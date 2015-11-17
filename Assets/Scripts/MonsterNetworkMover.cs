using UnityEngine;
using System.Collections;

public class MonsterNetworkMover : MonoBehaviour {
	private Vector3 correctMonsterPosition;
	private Quaternion correctMonsterRotation;
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
				transform.position = Vector3.Lerp(transform.position, correctMonsterPosition, Time.deltaTime * 5);
				transform.rotation = Quaternion.Lerp (transform.rotation, correctMonsterRotation, Time.deltaTime * 5);
				
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
			this.correctMonsterPosition = (Vector3) stream.ReceiveNext();
			this.correctMonsterRotation =  (Quaternion) stream.ReceiveNext();
		}
	}
}

