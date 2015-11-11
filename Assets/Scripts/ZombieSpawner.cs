using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour {
	public float radius = 1;
	public int maxZombieAmount = 20;
	public int maxSkeletonAmount = 5;
	public int maxMonsterAmount = 10;
	public int maxGolemAmount = 0;
	public GameObject zombieInstance;
	public GameObject skeletonInstance;
	public GameObject monsterInstance;
	public GameObject golemInstance;
	public GameObject player;
	public int currentZombieAmount = 0;
	public int currentSkeletonAmount = 0;
	public int currentMonsterAmount = 0;
	public int currentGolemAmount = 0;
	public bool playerIsInMaze = false;

	private int count = 0;
	// Use this for initialization
	void Start () {
		updateEnemyCount ();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (playerIsInMaze) {
			if (count >= 50) {
				count = 0;
				if(currentZombieAmount<maxZombieAmount){
					float direction = Random.Range (0f, 3.14f);
					float randomX = gameObject.transform.position.x + Mathf.Cos (direction) * radius;
					float randomZ = gameObject.transform.position.z + Mathf.Sin (direction) * radius;
					//replace above code with function finding random coords in adjacentRoom
					Instantiate (zombieInstance, new Vector3 (randomX, 6, randomZ), Quaternion.identity);
					currentZombieAmount++;
				}
				if(currentSkeletonAmount<maxSkeletonAmount){
					float direction = Random.Range (0f, 3.14f);
					float randomX = gameObject.transform.position.x + Mathf.Cos (direction) * radius;
					float randomZ = gameObject.transform.position.z + Mathf.Sin (direction) * radius;
					//replace above code with function finding random coords in adjacentRoom
					Instantiate (skeletonInstance, new Vector3 (randomX, 6, randomZ), Quaternion.identity);
					currentSkeletonAmount++;
				}
				if(currentMonsterAmount<maxMonsterAmount){
					float direction = Random.Range (0f, 3.14f);
					float randomX = gameObject.transform.position.x + Mathf.Cos (direction) * radius;
					float randomZ = gameObject.transform.position.z + Mathf.Sin (direction) * radius;
					//replace above code with function finding random coords in adjacentRoom
					Instantiate (monsterInstance, new Vector3 (randomX, 6, randomZ), Quaternion.identity);
					currentMonsterAmount++;
				}
				if(currentGolemAmount<maxGolemAmount){
					float direction = Random.Range (0f, 3.14f);
					float randomX = gameObject.transform.position.x + Mathf.Cos (direction) * radius;
					float randomZ = gameObject.transform.position.z + Mathf.Sin (direction) * radius;
					//replace above code with function finding random coords in adjacentRoom
					Instantiate (golemInstance, new Vector3 (randomX, 6, randomZ), Quaternion.identity);
					currentGolemAmount++;
				}
			}
			count+=1;
		}
	}

	void OnTriggerExit (Collider other)
	{	

	}

	void updateEnemyCount (){
		currentZombieAmount = 0;
		currentSkeletonAmount = 0;
		currentMonsterAmount = 0;
		currentGolemAmount = 0;
		GameObject[] arrayOfEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i=0; i<arrayOfEnemies.Length; i++) {
			//Insert code to skip enemies if not in adjacentRoom.
			string nameOfEnemy = arrayOfEnemies[i].name;
			if (nameOfEnemy == "Zombie"){
				currentZombieAmount+=1;
			}
			else if (nameOfEnemy == "Skeleton"){
				currentSkeletonAmount+=1;
			}
			else if (nameOfEnemy == "Monster"){
				currentMonsterAmount+=1;
			}
			else if (nameOfEnemy == "Golem"){
				currentGolemAmount+=1;
			}
			else{
				Debug.Log ("NO SUCH ENEMY: "+nameOfEnemy);
			}
		}
	}
	public void playerEntersMaze(){
		playerIsInMaze = true;
	}
	public void playerExitsMaze(){
		playerIsInMaze = false;
	}
}
