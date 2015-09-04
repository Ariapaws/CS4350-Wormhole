using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameCountDown : MonoBehaviour {

	public float initGameTime = 0;
	private float gameTime;
	public Text countDownDisplay;
	public GameObject player;
	public float freezeTime = 5f;
	public bool isCounting = false;
	AudioSource playerAudio;
	public AudioClip teleportClip;
	public AudioClip timeClip;

	// Use this for initialization
	void Start () {
		countDownDisplay.text = "You are back in base";
		playerAudio = GetComponent <AudioSource> ();
	}

	public void startCounting(){
		if(!isCounting){
			gameTime = initGameTime;
			isCounting = true;
		}
	}

	public void endCounting(){
		if(isCounting){
			isCounting = false;
			countDownDisplay.text = "You are back in base";
		}
	}

	public void AddTime(float time){
		if(isCounting){
			gameTime += time;
			playerAudio.clip = timeClip;
			playerAudio.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isCounting){
			gameTime -= Time.deltaTime;
			if(gameTime > 30f){
				countDownDisplay.text = "Time Left: " + gameTime.ToString ("0");
			} else {
				countDownDisplay.text = "Hurry Up! " + gameTime.ToString ("0");
			}

			if(gameTime <= 0f){
				countDownDisplay.text = "You are back in base";
				//Vector3 temp = new Vector3(-39.55f, 1.8f, -4.04f);
				Vector3 temp = new Vector3(-14.6f, 1.8f, -21.5f);
				player.transform.position = temp;
				playerAudio.clip = teleportClip;
				playerAudio.Play();
				endCounting();
			}

			if (player.transform.position.x>= 42f){
				GameObject notDes = GameObject.FindGameObjectWithTag ("NotDestroyed");
				Score holder = notDes.GetComponent<Score>();
				holder.UpdateScore();
				Application.LoadLevel("EndScreen");

			}

		}

	}
	
}
