using UnityEngine;
using System.Collections;

public class TODSyncScript : Photon.MonoBehaviour {
	private TOD todScript;
	void Start () {
		todScript = GetComponent<TOD>();
		GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ().todScript = todScript;
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext(todScript.slider);	
			stream.SendNext(todScript.slider2);
		}  else {
			todScript.slider = (float) stream.ReceiveNext();
			todScript.slider2 =  (float) stream.ReceiveNext();
		}
	}

}
