using UnityEngine;
using System.Collections;

public class PhotonNetworkManager : Photon.PunBehaviour {

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}

	void OnGUI () {
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
	}


	public override void OnConnectedToMaster ()
	{
		PhotonNetwork.JoinLobby ();
	}

	public override void OnJoinedLobby ()
	{
		RoomOptions roomOptions = new RoomOptions () {isVisible = true, maxPlayers = 10};
		PhotonNetwork.JoinOrCreateRoom("Main", roomOptions ,TypedLobby.Default);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
