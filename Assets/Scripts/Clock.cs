using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Clock : MonoBehaviour {
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
//
//  Simple Clock Script / Andre "AEG" Bürger / VIS-Games 2012
//
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------

    //-- set start time 00:00
    public int minutes = 0;
    public int hour = 0;
	public float timeIncrease = 30f;
    //-- time speed factor
    public float clockSpeed = 1.0f;     // 1.0f = realtime, < 1.0f = slower, > 1.0f = faster
	public bool clockUsed = false;
    //-- internal vars
    int seconds;
    float msecs;
    GameObject pointerSeconds;
    GameObject pointerMinutes;
    GameObject pointerHours;
	GameCountDown countDown;
	GameObject gameManager;
	GameObject player;
	public Text feedback;

void OnTriggerEnter (Collider other)
{
	if(other.gameObject == player && !clockUsed)
	{
		countDown.AddTime(timeIncrease);
		clockUsed = true;
		feedback.color = new Color(1,1,1,2);
		feedback.text = "You have gained "+timeIncrease.ToString()+" seconds.";
		Destroy (gameObject, 0f);
	}
}


void OnTriggerExit (Collider other)
{
	
}

//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
void Awake() 
{
    
	gameManager = GameObject.FindGameObjectWithTag ("GameManager");
	player = GameObject.FindGameObjectWithTag ("Player");
	countDown = gameManager.GetComponent <GameCountDown> ();
	pointerSeconds = transform.Find("rotation_axis_pointer_seconds").gameObject;
    pointerMinutes = transform.Find("rotation_axis_pointer_minutes").gameObject;
    pointerHours   = transform.Find("rotation_axis_pointer_hour").gameObject;

    msecs = 0.0f;
    seconds = 0;
	GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
	feedback = feedbackObject.GetComponent<Text>();
}


//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
void Update() 
{
    //-- calculate time
	
	transform.Rotate( 0, Time.deltaTime * 100, 0);

    msecs += Time.deltaTime * clockSpeed;
    if(msecs >= 1.0f)
    {
        msecs -= 1.0f;
        seconds++;
        if(seconds >= 60)
        {
            seconds = 0;
            minutes++;
            if(minutes > 60)
            {
                minutes = 0;
                hour++;
                if(hour >= 24)
                    hour = 0;
            }
        }
    }


    //-- calculate pointer angles
    float rotationSeconds = (360.0f / 60.0f)  * seconds;
    float rotationMinutes = (360.0f / 60.0f)  * minutes;
    float rotationHours   = ((360.0f / 12.0f) * hour) + ((360.0f / (60.0f * 12.0f)) * minutes);

    //-- draw pointers
    pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationSeconds);
    pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationMinutes);
    pointerHours.transform.localEulerAngles   = new Vector3(0.0f, 0.0f, rotationHours);

}
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
}
