using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Shop : MonoBehaviour {
	
	public bool canPurchase;
	public float distance;

	//public List<ShopItem> shopItems = new List<Shop>();

	public Text feedback;
    public GameObject player;

	// Use this for initialization
	void Start () {
		distance = 2.8f;
		canPurchase = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
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
            else if (player.GetComponent<PlayerPurchaseUI>().showPurchaseUI == false) {
                canPurchase = false;
            }
		} else 
			canPurchase = false;


        if (canPurchase == true) { 
            // Press 1 (upgrade WEAPON ATTACK) --------------------------------------
            //player.GetComponent<PlayerAssets>().currentCash
            if (Input.GetKeyUp(KeyCode.Alpha1)) {
                // 1st upgrade = +5
                if (CheckCost() == true) { player.GetComponent<PlayerAttack>().damagePerHit = 25; }
            }

            // Press 2 (upgrade WEAPON RANGE) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                // 1st upgrade = +0.5
                if (CheckCost() == true) { player.GetComponent<PlayerAttack>().range = 5.5f; }
            }

            // Press 3 (upgrade ARMOUR--health) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                // 1st upgrade = +50 health
                if (CheckCost() == true) { player.GetComponent<PlayerHealth>().startingHealth = 350; }
            }

            // Press 4 (upgrade SPEED) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                // 1st upgrade = +50 health
                //player.GetComponent<FirstPersonController>().startingHealth = 350;
            }

            // Press 5 (purchase POTION) --------------------------------------


        }
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

    bool CheckCost() {
        if (player.GetComponent<PlayerAssets>().currentCash >= 0)
        {
            return true;
        }
        else
            return false;
    }



}
