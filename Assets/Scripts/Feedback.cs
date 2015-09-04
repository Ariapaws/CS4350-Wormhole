using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Feedback : MonoBehaviour {
	Text feedback;
	// Use this for initialization
	void Start () {
		feedback = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		float fadeSpeed = 1f;
		if (feedback.color.a > 0){
			float alpha = feedback.color.a - fadeSpeed*Time.deltaTime;
			feedback.color = new Color(feedback.color.r, feedback.color.g, feedback.color.b, alpha);
		}
	}
}
