using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;
	private Maze mazeInstance;
	private bool pauseEnabled = false; 
	GameObject player;
	public Text feedback;
	public bool isNight=false;
    private int checkNightCount = 0;

//	private IEnumerator Start () {
//		//BeginGame();
//		mazeInstance = Instantiate(mazePrefab) as Maze;
//		yield return StartCoroutine(mazeInstance.Generate());
//
//	}
	
	// Use this for initialization
	void Awake () {
		GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
		feedback = feedbackObject.GetComponent<Text>();
	}
	
	private void Update () {
        if (checkNightCount == 5)
        {
            checkNightCount = 0;
            GameObject timeOfDayObject = GameObject.FindGameObjectWithTag("TOD");
            TOD todScript = (TOD)timeOfDayObject.GetComponent(typeof(TOD));
            float hr = todScript.Hour;
            //Debug.Log (hr + "isNight is " + isNight);
            if (isNight)
            {
                if (hr > 6 && hr < 18)
                {
                    feedback.color = new Color(1, 1, 1, 2);
                    feedback.text = "The sun has risen. Congrats for surviving another night.";
                    isNight = false;
                }
            }
            else
            {
                if (hr < 6 || hr > 18)
                {
                    feedback.color = new Color(1, 1, 1, 2);
                    feedback.text = "Night has fallen. Zombies have become tougher.";
                    isNight = true;
                }
            }
        } else
        {
            checkNightCount++;
        }
		

		/*
		if (Input.GetKeyDown(KeyCode.Escape)) {
			//check if game is already paused       
			if(pauseEnabled == true){
				//unpause the game
				Time.timeScale = 1;
				pauseEnabled = false;
			}
			
			//else if game isn't paused, then pause it
			else if(pauseEnabled == false){
				Time.timeScale = 0;
				pauseEnabled = true;
			}
		}
		*/


	}

	void OnTriggerExit (Collider other)
	{
		Debug.Log("OUT");
		if(other.gameObject == player){
			other.gameObject.GetComponent<PlayerHealth>().TakeDamage(300);
		}
	}
	private void BeginGame () {
		mazeInstance = Instantiate(mazePrefab) as Maze;
		StartCoroutine(mazeInstance.Generate());
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	private void RestartGame () {
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		BeginGame();
	}
}