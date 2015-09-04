using UnityEngine;
using System.Collections;

public class ZombieDome : MonoBehaviour {
	public float radius;
	public int zombieAmount;
	public GameObject zombieInstance;
	public int currentAmount = 0;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < zombieAmount; i++) {
			float direction = Random.Range (0f, 3.14f);
			float randomX = gameObject.transform.position.x + Mathf.Cos(direction) * radius;
			float randomZ = gameObject.transform.position.z + Mathf.Sin(direction) * radius;
			Instantiate(zombieInstance, new Vector3(randomX, 6, randomZ), Quaternion.identity);
			currentAmount++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(currentAmount < zombieAmount){
			float direction = Random.Range (0f, 3.14f);
			float randomX = gameObject.transform.position.x + Mathf.Cos(direction) * radius;
			float randomZ = gameObject.transform.position.z + Mathf.Sin(direction) * radius;
			Instantiate(zombieInstance, new Vector3(randomX, 6, randomZ), Quaternion.identity);
			currentAmount++;
		}

	}

	void OnTriggerExit (Collider other)
	{	
		Debug.Log(other.gameObject.tag);
		if(other.gameObject.tag == "randomZombie")
		{
			Destroy(other.gameObject);
			currentAmount = currentAmount - 1;
		}
	}
}
