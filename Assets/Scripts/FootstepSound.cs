using UnityEngine;
using System.Collections;

public class FootstepSound: MonoBehaviour {

	CharacterController cc;
	AudioSource playerAudio;
	public AudioClip footStepSound;

	// Use this for initialization
	void Start () {
		playerAudio = GetComponent <AudioSource> ();
		cc = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (cc.velocity.magnitude > 1.5f && playerAudio.isPlaying == false) {
			playerAudio.clip = footStepSound;
			playerAudio.Play();
		}
	}
}
