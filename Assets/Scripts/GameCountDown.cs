using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameCountDown : MonoBehaviour {

	public float initGameTime = 180;
	private float gameTime;
	public Text textDisplay;
	public GameObject player;

    // Use this for initialization
    void Start()
    {
        textDisplay.text = "";
    }

    public Text getText()
    {
        return textDisplay;
    }

	public void AddTime(float time){ //old function
	}
	
	// Update is called once per frame
	void Update () {
        if (player.transform.position.x >= 79f)
        {
            GameObject notDes = GameObject.FindGameObjectWithTag("NotDestroyed");
            Score holder = notDes.GetComponent<Score>();
            holder.UpdateScore();
            Application.LoadLevel("EndScreen");

        }
    }
	
}
