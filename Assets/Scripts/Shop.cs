using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Shop : MonoBehaviour {
	
	public bool canPurchase;
	public float distance;
	public GameObject player;

	//public List<ShopItem> shopItems = new List<Shop>();

	public Text feedback; 

	// Use this for initialization
	void Start () {
		distance = 2.8f;
		canPurchase = false;

        GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
        feedback = feedbackObject.GetComponent<Text>();

		InstantiateShop ();


	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (this.gameObject.transform.position, player.transform.position) < distance) {
			Debug.Log("Player is near.");
            feedback.color = new Color(0, 1, 0, 2);
            feedback.text = "You are near the shop. Press P to purchase.";
			if (player.GetComponent<PlayerPurchaseUI>().showPurchaseUI == true) {
				Debug.Log("Player can make purchase.");
				Debug.Log(player.GetComponent<PlayerPurchaseUI>().showPurchaseUI);
				canPurchase = true;
			}
		} else 
			canPurchase = false;
	}

	void InstantiateShop () {
		//fill shop array with items
		// type, weapon name, attack damage, durability, cost
		// type, invisible items name, upgrade number, durability, cost, upgrade name

		// clear list
		//List<Shop> shopItems = new List<Shop>();
		/* shopItems.Clear();

		//items type 1 (WEAPONS)
		shopItems.Add (new Shop (1, "Dagger", 20, 500, 20));
		shopItems.Add (new Shop (1, "Sword", 40, 100, 60));

		//items type 2 (invisible items)
		shopItems.Add (new Shop (2, "Helmet", 100, 100, 100, "health"));
		shopItems.Add (new Shop (2, "Boots of Speed", 2, 100, 50, "speed"));
		shopItems.Add (new Shop (2, "Armor", 100, 100, 200, "health"));
		shopItems.Add (new Shop (2, "Ring", 1.1, 50, 200, "weapon_power"));

		shopItems.Add (new Shop (2, "Potion", 50, 1, 20, "health")); */
	}



}
