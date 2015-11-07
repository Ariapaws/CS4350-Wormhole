using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	public PlayerAssets assets;
	public int score = 0;

	// Use this for initialization
	void Start () {
//		GameObject player = GameObject.FindGameObjectWithTag ("Player");
//		assets = player.GetComponent <PlayerAssets> ();
	}

	void Awake() {
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	public void UpdateScore () {
		score = assets.currentCash;
	}
}
