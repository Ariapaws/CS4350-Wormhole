using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameCountDown : MonoBehaviour {

	public float initGameTime = 180;
	private float gameTime;
	public Text textDisplay;
	public GameObject player;
	public bool isShopOpen = false;

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
        /*
        float fadeSpeed = 0.8f;
        if (textDisplay.color.a > 0)
        {
            float alpha = textDisplay.color.a - fadeSpeed * Time.deltaTime;
            textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alpha);
        }
        */
        if (player.transform.position.x >= 79f)
        {
            GameObject notDes = GameObject.FindGameObjectWithTag("NotDestroyed");
            Score holder = notDes.GetComponent<Score>();
            holder.UpdateScore();
            Application.LoadLevel("EndScreen");

        }

    }
	
}
