using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections;

public class PlayerPurchaseUI : MonoBehaviour {

	public bool showPurchaseUI;
	public bool toggle;

	public Text feedback;
	public GameObject HUDcanvas = GameObject.FindGameObjectWithTag("Shop");
	
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
			if(toggle==true) {
				Debug.Log ("toggled 0");
				showPurchaseUI = false;
				//hide purchase UI
			}
			toggle = !toggle;
		}
	}

	void PurchaseUI(){
		
		

	}
}
