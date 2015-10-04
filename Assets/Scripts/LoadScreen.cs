using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadScreen : MonoBehaviour {	
	//public GameObject background;
	public GameObject progressBar;
	public GameObject textObject;
	
	private int loadProgress = 0;
	
	// Use this for initialization
	void Start () {
		//background.SetActive (false);
		progressBar.SetActive (false);
		textObject.SetActive (false);
	}
	
	void Update(){
		StartCoroutine(DisplayLoadingScreen("MainGame"));
	}
	
	IEnumerator DisplayLoadingScreen(string level){
		//background.SetActive (true);
		progressBar.SetActive (true);
		textObject.SetActive (true);
		
		progressBar.transform.localScale = new Vector3 (loadProgress, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

		textObject.GetComponent<Text>().text = "Loading " + loadProgress + "%";
		
		AsyncOperation async = Application.LoadLevelAsync(level);
		// async.allowSceneActivation = false;
		while(!async.isDone){
			loadProgress = (int)(async.progress * 100);
			textObject.GetComponent<Text>().text = "Loading " + loadProgress + "%";
			progressBar.transform.localScale = new Vector3(async.progress, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

			yield return null;
		}
		// async.allowSceneActivation = true;
	}

}
