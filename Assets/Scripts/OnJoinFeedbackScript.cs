using UnityEngine;
using System.Collections;

public class OnJoinFeedbackScript : MonoBehaviour {
	public UnityEngine.UI.Text textUI;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[PunRPC]
	void newPlayerNameBroadcast(string playerName) {
		textUI.color = new Color (0, 1, 0, 2);
		textUI.text = playerName + " has joined the game!"; 
	}

	[PunRPC]
	void playerLeaveGameBroadcast(string playerName) {
		textUI.color = new Color (0, 1, 0, 2);
		textUI.text = playerName + " has left the game!"; 
	}
}

