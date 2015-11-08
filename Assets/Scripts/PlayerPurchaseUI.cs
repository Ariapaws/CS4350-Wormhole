using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Collections;

public class PlayerPurchaseUI : MonoBehaviour {

	public bool showPurchaseUI;
	public bool toggle;

	public GameObject canvas;
    public Text feedback;
	private ChatScript chatscript;


	
	// Use this for initialization
	void Start () {
		chatscript = GameObject.FindGameObjectWithTag ("ChatBox").GetComponent<ChatScript> ();
		showPurchaseUI = false;
		toggle = false;

        //playerInventory = gameObject.Find("Player").GetComponent(Inventory_GUI)
        Text[] texts = canvas.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].enabled = false;
        }
        Image[] images = canvas.GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = false;
        }

    }
	
	// Update is called once per frame
	void Update () {
		if (!chatscript.isActive) {
			if (Input.GetKeyUp (KeyCode.P)) {
				if (toggle == false) {
					Debug.Log ("P pressed!");
					Debug.Log ("toggled 1");
					showPurchaseUI = true;
					PurchaseUI ();
				} else if (toggle == true) {
					Debug.Log ("toggled 0");
					showPurchaseUI = false;
					// show or hide purchase UI
					PurchaseUI ();

				}
				toggle = !toggle;
			}
		}
	}

    void PurchaseUI()
    {
        if (showPurchaseUI == true) {
            //canvas.GetComponent<Image>().enabled = true;
            //canvas.GetComponentInChildren<Text>().enabled = true;

            Text[] texts = canvas.GetComponentsInChildren<Text>();
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].enabled = true;
            }
            Image[] images = canvas.GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                images[i].enabled = true;
            }
            // show table of available purchases
        }

        if (showPurchaseUI == false) {
            //canvas.GetComponent<Image>().enabled = false;
            //canvas.GetComponentInChildren<Text>().enabled = false;
            Text[] texts = canvas.GetComponentsInChildren<Text>();
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].enabled = false;
            }
            Image[] images = canvas.GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                images[i].enabled = false;
            }
        }
    }
}
