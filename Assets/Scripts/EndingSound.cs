using UnityEngine;
using System.Collections;

public class EndingSound : MonoBehaviour {

	public AudioClip endingSound;
	AudioSource audio;
	// Use this for initialization
	void Start () {
		audio = GetComponent <AudioSource> ();
		audio.clip = endingSound;
		audio.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
