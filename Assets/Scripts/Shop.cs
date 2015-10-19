using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Shop : MonoBehaviour {
	
	public bool canPurchase;
	public float distance;
    public Text feedback;
    public GameObject player;


    public List<Upgrades> WeaponDamage;
    public List<Upgrades> WeaponRange;
    public List<Upgrades> WeaponSpeed;
    public List<Upgrades> Armour;

    public int currentWeaponDamageLevel;


	// Use this for initialization
	void Start () {
		distance = 2.8f;
		canPurchase = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
        feedback = feedbackObject.GetComponent<Text>();

        currentWeaponDamageLevel = 0;

		InstantiateShop ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (this.gameObject.transform.position, player.transform.position) < distance) {
            feedback.color = new Color(0, 1, 0, 2);
            feedback.text = "You are near the shop. Press P to purchase.";
			if (player.GetComponent<PlayerPurchaseUI>().showPurchaseUI == true) {
				canPurchase = true;
			}
            else if (player.GetComponent<PlayerPurchaseUI>().showPurchaseUI == false) {
                canPurchase = false;
            }
		} else 
			canPurchase = false;


        if (canPurchase == true) { 
            // Press 1 (upgrade WEAPON ATTACK) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha1)) {
                for (int i = 0; i < WeaponDamage.Count; i++) {
                    Debug.Log("Checking if can upgrade..");
                    if (WeaponDamage[i].level > currentWeaponDamageLevel) {
                        Debug.Log("Can upgrade.");
                        Debug.Log(WeaponDamage[i].cost);
                        if (CheckCost(WeaponDamage[i].cost) == true) {
                            Debug.Log("Enough gold.");
                            player.GetComponent<PlayerAttack>().damagePerHit += WeaponDamage[i].upgradeAmount;
                            currentWeaponDamageLevel = WeaponDamage[i].level;
                            break;
                        }
                    }
                }
            }

            // Press 2 (upgrade WEAPON RANGE) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                //if (CheckCost() == true) { player.GetComponent<PlayerAttack>().range = 5.5f; }
            }

            // Press 3 (upgrade ATTACK SPEED) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                //if (CheckCost() == true) { player.GetComponent<PlayerAttack>().attackSpeed = 1.2f; }
            }

            // Press 4 (upgrade ARMOUR--health) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                //if (CheckCost() == true) { player.GetComponent<PlayerHealth>().startingHealth = 350; }
            }



            // Press 5 (purchase POTION) --------------------------------------


        }
	}

	void InstantiateShop () {
		//fill shop array with items
		// button type, upgrade name, weapon level--still upgradable?, cost, upgrade amount 


		// STEP 1: clear list
	    WeaponDamage = new List<Upgrades>();
		WeaponDamage.Clear();

        
        // STEP 2: fill list with items info
		//items type 1 (upgrades)
        //ALPHA 1
        WeaponDamage.Add(new Upgrades(1, "Weapon Damage", 1, 0, 5));
        WeaponDamage.Add(new Upgrades(1, "Weapon Damage", 2, 0, 5));
        //ALPHA 2
		//shopItems.Add (new Shop (2, "Weapon Range", 1, 100, 0.5f));
        //ALPHA 3
		//shopItems.Add (new Shop (3, "Armor", 1, 100, 50));
        //ALPHA 4
		//shopItems.Add (new Shop (4, "Boots", 1, 100, 0.5f));
               
        //items type 2 (consumables)
        //ALPHA 5
		//shopItems.Add (new Shop (5, "Potion", 1, 30, 50));

	}

    bool CheckCost(int upgradeCost) {
        if (upgradeCost <= player.GetComponent<PlayerAssets>().currentCash)
        {
            return true;
        }
        else
            return false;
    }



}
