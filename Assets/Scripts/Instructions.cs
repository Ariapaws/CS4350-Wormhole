using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Instructions : MonoBehaviour {
    Text bottomText;
    private int count;
    AsyncOperation async;
    private bool hasStartedLoad = false;

    private string loading = "Loading";
    private string spaces = "                     ";
    private string dots = "";

    // Use this for initialization
    void Start () {
        bottomText = GameObject.FindGameObjectWithTag("Finish").GetComponent<Text>();
        count = 0;
	}

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            bottomText.text = loading+spaces;
            async = Application.LoadLevelAsync("MainGame");
            hasStartedLoad = true;
            count = 0;
        }
        if (hasStartedLoad)
        {
            count++;
            Debug.Log(async.isDone);
        }
        if (hasStartedLoad && !async.isDone)
        {
            if (count % 10 == 0 && count < 200)
            {
                spaces = spaces.Substring(1);
                dots += ".";
                bottomText.text = loading+dots+spaces;
            }
        }
        
    }
}
