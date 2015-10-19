using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections;

public class PlayerPurchaseUI : MonoBehaviour {

	public bool showPurchaseUI;
	public bool toggle;

    public GameObject canvas = GameObject.FindGameObjectWithTag("Shop");
	public Text feedback;
	
	// Use this for initialization
	void Start () {
		showPurchaseUI = false;
		toggle = false;

		feedback = GetComponent<Text>();
		//playerInventory = gameObject.Find("Player").GetComponent(Inventory_GUI)

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.P)) {
			if(toggle==false) {
				Debug.Log ("P pressed!");
				Debug.Log ("toggled 1");
				showPurchaseUI = true;
				PurchaseUI ();
			}
			else if(toggle==true) {
				Debug.Log ("toggled 0");
				showPurchaseUI = false;
                // show or hide purchase UI
                PurchaseUI ();

			}
			toggle = !toggle;
		}
	}

    void PurchaseUI()
    {
        if (showPurchaseUI == true) {
            canvas.GetComponent<Image>().enabled = true;
            // show table of available purchases
        }

        if (showPurchaseUI == false) {
            canvas.GetComponent<Image>().enabled = false;
        }
    }
}
