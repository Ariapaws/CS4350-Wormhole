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
    public int currentWeaponRangeLevel;
    public int currentWeaponSpeedLevel;
    public int currentArmourLevel;

	// Use this for initialization
	void Start () {
		distance = 2.8f;
		canPurchase = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
        feedback = feedbackObject.GetComponent<Text>();

        currentWeaponDamageLevel = 0;
        currentWeaponRangeLevel = 0;
        currentWeaponSpeedLevel = 0;
        currentArmourLevel = 0;

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
                    Debug.Log("Checking if can upgrade damage..");
                    if (WeaponDamage[i].level > currentWeaponDamageLevel) {
                        Debug.Log("Can upgrade damage.");
                        if (CheckCost(WeaponDamage[i].cost) == true) {
                            Debug.Log("Enough gold for damage.");
                            player.GetComponent<PlayerAttack>().damagePerHit += (int) WeaponDamage[i].upgradeAmount;
                            currentWeaponDamageLevel = WeaponDamage[i].level;
                            break;
                        }
                    }
                }
            }

            // Press 2 (upgrade WEAPON RANGE) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha2)) {
                for (int i = 0; i < WeaponRange.Count; i++) {
                    Debug.Log("Checking if can upgrade range..");
                    if (WeaponRange[i].level > currentWeaponRangeLevel) {
                        Debug.Log("Can upgrade range.");
                        if (CheckCost(WeaponRange[i].cost) == true) {
                            Debug.Log("Enough gold for range.");
                            player.GetComponent<PlayerAttack>().range += WeaponRange[i].upgradeAmount;
                            currentWeaponRangeLevel = WeaponRange[i].level;
                            break;
                        }
                    }
                }
            }

            // Press 3 (upgrade ATTACK SPEED) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha3)) {
                for (int i = 0; i < WeaponSpeed.Count; i++) {
                    Debug.Log("Checking if can upgrade speed..");
                    if (WeaponSpeed[i].level > currentWeaponSpeedLevel) {
                        Debug.Log("Can upgrade speed.");
                        if (CheckCost(WeaponSpeed[i].cost) == true) {
                            Debug.Log("Enough gold for speed.");
                            player.GetComponent<PlayerAttack>().attackSpeed += WeaponSpeed[i].upgradeAmount;
                            currentWeaponSpeedLevel = WeaponSpeed[i].level;
                            break;
                        }
                    }
                }
            }

            // Press 4 (upgrade ARMOUR--health) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha4)) {
                //if (CheckCost() == true) { player.GetComponent<PlayerHealth>().startingHealth = 350; }
                for (int i = 0; i < Armour.Count; i++) {
                    Debug.Log("Checking if can upgrade armour..");
                    if (Armour[i].level > currentArmourLevel) {
                        Debug.Log("Can upgrade armour.");
                        if (CheckCost(Armour[i].cost) == true) {
                            Debug.Log("Enough gold for armour.");
                            player.GetComponent<PlayerHealth>().startingHealth += (int) Armour[i].upgradeAmount;
                            currentArmourLevel = Armour[i].level;
                            break;
                        }
                    }
                }
            }

            // Press 5 (purchase POTION) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                if (CheckCost(50) == true) {
                    Debug.Log("Enough gold for potion.");
                    player.GetComponent<PlayerAssets>().numOfPotions += 1;
                }
            }

        }
	}

	void InstantiateShop () {
		//fill shop array with items

		// STEP 1: clear list
	    WeaponDamage = new List<Upgrades>();
		WeaponDamage.Clear();
        WeaponRange = new List<Upgrades>();
        WeaponRange.Clear();
        WeaponSpeed = new List<Upgrades>();
        WeaponSpeed.Clear();
        Armour = new List<Upgrades>();
        Armour.Clear();
  
        // STEP 2: fill list with items info
		//items type 1 (upgrades)
        // button type, upgrade name, weapon level--still upgradable?, cost, upgrade amount 
        //ALPHA 1
        WeaponDamage.Add(new Upgrades(1, "Weapon Damage", 1, 0, 5));
        WeaponDamage.Add(new Upgrades(1, "Weapon Damage", 2, 0, 5));
        //ALPHA 2
        WeaponRange.Add(new Upgrades(2, "Weapon Range", 1, 0, 0.5f));
        WeaponRange.Add(new Upgrades(2, "Weapon Range", 2, 0, 0.5f));
        //ALPHA 3
        WeaponSpeed.Add(new Upgrades(2, "Weapon Speed", 1, 0, 0.2f));
        WeaponSpeed.Add(new Upgrades(2, "Weapon Speed", 2, 0, 0.1f));
        //ALPHA 4
        Armour.Add(new Upgrades(3, "Armor", 1, 0, 50));
        Armour.Add(new Upgrades(3, "Armor", 2, 0, 50));
               
        //items type 2 (consumables)
        //ALPHA 5
		//shopItems.Add (new Shop (5, "Potion", 1, 30, 50));

	}

    bool CheckCost(int upgradeCost) {
        if (upgradeCost <= player.GetComponent<PlayerAssets>().currentCash)
        {
            player.GetComponent<PlayerAssets>().currentCash -= upgradeCost;
            return true;
        }
        else
            return false;
    }



}
