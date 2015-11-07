using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFader : MonoBehaviour {
	public Text textUI;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float fadeSpeed = 1f;
		if (textUI.color.a > 0){
			float alpha = textUI.color.a - fadeSpeed*Time.deltaTime;
			textUI.color = new Color(textUI.color.r, textUI.color.g, textUI.color.b, alpha);
		}
	}
}
