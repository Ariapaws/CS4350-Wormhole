using UnityEngine;
using System.Collections;

public class StartScreenScript : MonoBehaviour {
	
	public void OnClickPlay(){
		Application.LoadLevel("MainGame");
	}

}