using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatMessagesScript : MonoBehaviour {
	private List<string> messages;
	public UnityEngine.UI.Text textUI;
	// Use this for initialization
	void Start () {
		textUI.text = "";
		messages = new List<string> ();
	}
	
	// Update is called once per frame
	void Update () {
//		foreach (string message in messages) {
//			textUI.text = message;
//		}
	}

	[PunRPC]
	void UpdateMessage (string playerName, string message) {
//		messages.Add(playerName + ": " + message);
//		Debug.Log(playerName + ": " + message);

		textUI.color = new Color (189, 255, 255, 2);
		textUI.text = playerName + ": " + message;
	}

}
