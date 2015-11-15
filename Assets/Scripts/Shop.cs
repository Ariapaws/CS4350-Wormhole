using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Shop : MonoBehaviour {
	
	public bool canPurchase;
	public float distance;
    public Text feedback;
    public GameCountDown feedbackScript;
    public GameObject player;


    public List<Upgrades> WeaponDamage;
    public List<Upgrades> WeaponRange;
    public List<Upgrades> WeaponSpeed;
    public List<Upgrades> Armour;

    public int currentWeaponDamageLevel;
    public int currentWeaponRangeLevel;
    public int currentWeaponSpeedLevel;
    public int currentArmourLevel;

    public GameObject DamageUpgrade;
    public GameObject DamageCost;
    public GameObject SpeedUpgrade;
    public GameObject SpeedCost;
    public GameObject RangeUpgrade;
    public GameObject RangeCost;
    public GameObject ArmorUpgrade;
    public GameObject ArmorCost;

    GameObject currDisplayDamage;
    GameObject currDisplayRange;
    GameObject currDisplaySpeed;

    AudioSource audio;
    public AudioClip purchaseClip;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();

        DamageUpgrade = GameObject.FindGameObjectWithTag("damage-upgrade");
        DamageCost = GameObject.FindGameObjectWithTag("damage-cost");
        SpeedUpgrade = GameObject.FindGameObjectWithTag("speed-upgrade");
        SpeedCost = GameObject.FindGameObjectWithTag("speed-cost");
        RangeUpgrade = GameObject.FindGameObjectWithTag("range-upgrade");
        RangeCost = GameObject.FindGameObjectWithTag("range-cost");
        ArmorUpgrade = GameObject.FindGameObjectWithTag("armor-upgrade");
        ArmorCost = GameObject.FindGameObjectWithTag("armor-cost");

        currDisplayDamage = GameObject.FindGameObjectWithTag("CurrentStats-Damage");
        currDisplayRange = GameObject.FindGameObjectWithTag("CurrentStats-Range");
        currDisplaySpeed = GameObject.FindGameObjectWithTag("CurrentStats-Speed");

		distance = 2.8f;
		canPurchase = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
        //feedback = feedbackObject.GetComponent<Text>();
        feedback = feedbackScript.getText();

        currentWeaponDamageLevel = 0;
        currentWeaponRangeLevel = 0;
        currentWeaponSpeedLevel = 0;
        currentArmourLevel = 0;

		InstantiateShop ();
	}
	
	// Update is called once per frame
	void Update () {
        // if near shop, can purchase
		if (Vector3.Distance (this.gameObject.transform.position, player.transform.position) < distance) {

            feedback.color = new Color(0, 1, 0, 2);
            
			if (player.GetComponent<PlayerPurchaseUI>().showPurchaseUI == true) {
                feedback.text = "Press buttons 1-5 to purchase upgrades/items.";
                canPurchase = true;
			}
            else if (player.GetComponent<PlayerPurchaseUI>().showPurchaseUI == false) {
                feedback.text = "You are near the shop. Press P to purchase.";
                canPurchase = false;
            }
		} else {
            feedback.color = new Color(0, 255, 255, 2);
            if (player.GetComponent<PlayerPurchaseUI>().showPurchaseUI == true)
            {
                feedback.text = "Go back to the shop in base to purchase items";
            }
            else if (player.GetComponent<PlayerPurchaseUI>().showPurchaseUI == false)
            {
                feedback.text = "Press P to view upgrades/items.";
            }
            canPurchase = false;
        }

        //updating purchase UI
        DamageUpgrade.GetComponent<Text>().text = "+" + WeaponDamage[currentWeaponDamageLevel + 1].upgradeAmount + " Damage";
        DamageCost.GetComponent<Text>().text = WeaponDamage[currentWeaponDamageLevel+1].cost + " G";
        SpeedUpgrade.GetComponent<Text>().text = "+" + WeaponSpeed[currentWeaponSpeedLevel + 1].upgradeAmount + " Speed";
        SpeedCost.GetComponent<Text>().text = WeaponSpeed[currentWeaponSpeedLevel + 1].cost + " G";
        RangeUpgrade.GetComponent<Text>().text = "+" + WeaponRange[currentWeaponRangeLevel + 1].upgradeAmount + " Range";
        RangeCost.GetComponent<Text>().text = WeaponRange[currentWeaponRangeLevel + 1].cost + " G";
        ArmorUpgrade.GetComponent<Text>().text = "+" + Armour[currentArmourLevel + 1].upgradeAmount + " Health";
        ArmorCost.GetComponent<Text>().text = Armour[currentArmourLevel + 1].cost + " G";

        //update display UI
        currDisplayDamage.GetComponent<Text>().text = "" + player.GetComponent<PlayerAttack>().damagePerHit;
        currDisplayRange.GetComponent<Text>().text = "" + player.GetComponent<PlayerAttack>().range;
        currDisplaySpeed.GetComponent<Text>().text = "" + player.GetComponent<PlayerAttack>().attackSpeed;

        // purchasing system
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
                            feedback.text = "Upgraded Weapon Damage.";
                            audio.clip = purchaseClip;
                            audio.Play();
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
                            feedback.text = "Upgraded Attack Range.";
                            audio.clip = purchaseClip;
                            audio.Play();
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
                            feedback.text = "Upgraded Attack Speed.";
                            audio.clip = purchaseClip;
                            audio.Play();
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
                            feedback.text = "Upgraded Armour.";
                            audio.clip = purchaseClip;
                            audio.Play();
                            break;
                        }
                    }
                }
            }

            // Press 5 (purchase POTION) --------------------------------------
            if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                if (CheckCost(0) == true) {
                    Debug.Log("Enough gold for potion.");
                    player.GetComponent<PlayerAssets>().numOfPotions += 1;
                    feedback.text = "Purchase completed.";
                    audio.clip = purchaseClip;
                    audio.Play();
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
        WeaponDamage.Add(new Upgrades(1, "Weapon Damage", 1, 100, 3));
        WeaponDamage.Add(new Upgrades(1, "Weapon Damage", 2, 120, 3));
        WeaponDamage.Add(new Upgrades(1, "Weapon Damage", 3, 140, 3));
        WeaponDamage.Add(new Upgrades(1, "Weapon Damage", 4, 180, 5));
        WeaponDamage.Add(new Upgrades(1, "Weapon Damage", 5, 220, 5));
        WeaponDamage.Add(new Upgrades(1, "Weapon Damage", 6, 300, 10));
        //ALPHA 2
        WeaponRange.Add(new Upgrades(2, "Weapon Range", 1, 200, 0.2f));
        WeaponRange.Add(new Upgrades(2, "Weapon Range", 2, 200, 0.2f));
        WeaponRange.Add(new Upgrades(2, "Weapon Range", 3, 300, 0.4f));
        WeaponRange.Add(new Upgrades(2, "Weapon Range", 4, 450, 0.5f));
        //ALPHA 3
        WeaponSpeed.Add(new Upgrades(2, "Weapon Speed", 1, 200, 0.1f));
        WeaponSpeed.Add(new Upgrades(2, "Weapon Speed", 2, 200, 0.1f));
        WeaponSpeed.Add(new Upgrades(2, "Weapon Speed", 3, 300, 0.2f));
        WeaponSpeed.Add(new Upgrades(2, "Weapon Speed", 4, 450, 0.2f));
        //ALPHA 4
        Armour.Add(new Upgrades(3, "Armor", 1, 50, 20));
        Armour.Add(new Upgrades(3, "Armor", 2, 100, 20));
        Armour.Add(new Upgrades(3, "Armor", 3, 150, 30));
        Armour.Add(new Upgrades(3, "Armor", 4, 200, 30));
        Armour.Add(new Upgrades(3, "Armor", 5, 250, 50));
        Armour.Add(new Upgrades(3, "Armor", 6, 500, 70));
               
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
