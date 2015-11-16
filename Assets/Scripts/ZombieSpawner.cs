using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour {
	public float radius = 1;
	public int maxZombieAmount = 10;
	public int maxSkeletonAmount = 2;
	public int maxMonsterAmount = 3;
	public int maxGolemAmount = 0;
	public GameObject zombieInstance;
	public GameObject skeletonInstance;
	public GameObject monsterInstance;
	public GameObject golemInstance;
	public GameObject player;
	public GameObject maze;
	public int currentZombieAmount = 0;
	public int currentSkeletonAmount = 0;
	public int currentMonsterAmount = 0;
	public int currentGolemAmount = 0;
	public bool playerIsInMaze = false;
	//private List<MazeRoom> listOfRooms;

	private int count = 0;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (playerIsInMaze) {
			if (count >= 100) {
				Debug.Log (player.gameObject.transform.position);
				Debug.Log (getMazeCoordsFromWorldCoords(player.gameObject.transform.position).x);
				Debug.Log (getMazeCoordsFromWorldCoords(player.gameObject.transform.position).z);

				MazeCell myCell = maze.GetComponent<Maze>().GetCell(getMazeCoordsFromWorldCoords(player.gameObject.transform.position));
				List<MazeRoom> listOfRooms = maze.GetComponent<Maze>().getAdjacentRooms(myCell);
				updateEnemyCount(listOfRooms);
				count = 0;
				if(currentZombieAmount<maxZombieAmount){
					MazeRoom randRoom = getRandomRoom(listOfRooms);
					MazeCell randCell = randRoom.getRandomCell();
					Vector3 randPos = getWorldCoordsFromMazeCoords(randCell.coordinates);
					Instantiate (zombieInstance, randPos, Quaternion.identity);
					Debug.Log("Zombie Spawn!");
					currentZombieAmount++;
				}
				if(currentSkeletonAmount<maxSkeletonAmount){
					MazeRoom randRoom = getRandomRoom(listOfRooms);
					MazeCell randCell = randRoom.getRandomCell();
					Vector3 randPos = getWorldCoordsFromMazeCoords(randCell.coordinates);
					Instantiate (skeletonInstance, randPos, Quaternion.identity);
					currentSkeletonAmount++;
					Debug.Log("Skele Spawn!");

				}
				if(currentMonsterAmount<maxMonsterAmount){
					MazeRoom randRoom = getRandomRoom(listOfRooms);
					MazeCell randCell = randRoom.getRandomCell();
					Vector3 randPos = getWorldCoordsFromMazeCoords(randCell.coordinates);
					Instantiate (monsterInstance, randPos, Quaternion.identity);
					currentMonsterAmount++;
					Debug.Log("Monstaaaa Spawn!");
				}
				if(currentGolemAmount<maxGolemAmount){
					MazeRoom randRoom = getRandomRoom(listOfRooms);
					MazeCell randCell = randRoom.getRandomCell();
					Vector3 randPos = getWorldCoordsFromMazeCoords(randCell.coordinates);
					Instantiate (golemInstance, randPos, Quaternion.identity);
					currentGolemAmount++;
					Debug.Log("YOU SHOULDNT BE Spawned!");
				}
			}
			count+=1;
		}
	}
	

	void updateEnemyCount (List<MazeRoom> listOfRooms){
		currentZombieAmount = 0;
		currentSkeletonAmount = 0;
		currentMonsterAmount = 0;
		currentGolemAmount = 0;
		GameObject[] arrayOfEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i=0; i<arrayOfEnemies.Length; i++) {
			//Insert code to skip enemies if not in adjacentRoom.
			GameObject currEnemy = arrayOfEnemies[i];
			IntVector2 currEnemyPos = new IntVector2((int)currEnemy.transform.position.x,(int)currEnemy.transform.position.z);
			if(enemyIsInAdjRoom(listOfRooms, currEnemyPos)){
				string nameOfEnemy = currEnemy.name;
				if (nameOfEnemy == "Zombie(Clone)"){
					currentZombieAmount+=1;
				}
				else if (nameOfEnemy == "Skeleton(Clone)"){
					currentSkeletonAmount+=1;
				}
				else if (nameOfEnemy == "Monster(Clone)"){
					currentMonsterAmount+=1;
				}
				else if (nameOfEnemy == "Golem(Clone)"){
					currentGolemAmount+=1;
				}
				else{
					Debug.Log ("NO SUCH ENEMY: "+nameOfEnemy);
				}
			}
		}
	}
	public MazeRoom getRandomRoom(List<MazeRoom> listOfRooms)
	{
		int roomNum = listOfRooms.Count;
		int randRoomIndex = Random.Range(1, roomNum + 1) - 1;
		return listOfRooms[randRoomIndex];
	}
	
	public bool enemyIsInAdjRoom(List<MazeRoom> listOfRooms, IntVector2 enemyPos){
		for (int i =0; i<listOfRooms.Count; i++) {
			if(listOfRooms[i].isInRoom(enemyPos)){
				return true;
			}
		}
		return false;
	}
	public void playerEntersMaze(){
		playerIsInMaze = true;
	}
	public void playerExitsMaze(){
		playerIsInMaze = false;
	}
	public IntVector2 getMazeCoordsFromWorldCoords(Vector3 pos){
		int mazeX = (int)(pos.x/4f + 19.5f);
		int mazeZ = (int)(pos.z/4f + 19.5f);
		return new IntVector2 (mazeX, mazeZ);
	}
	public Vector3 getWorldCoordsFromMazeCoords(IntVector2 pos){
		float worldX = ((float)pos.x - 19.5f)*4f;
		float worldZ = ((float)pos.z - 19.5f)*4f;
		return new Vector3 (worldX, 6, worldZ);
	}
}
