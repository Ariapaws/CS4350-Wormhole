using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ZombieSpawner : MonoBehaviour {
	public float radius = 1;
	public int maxZombieAmount = 10;
	public int maxSkeletonAmount = 2;
	public int maxMonsterAmount = 3;
	public int maxGolemAmount = 0;
	public GameObject skeletonInstance;
	public GameObject monsterInstance;
	public GameObject golemInstance;
	public GameObject[] players;
	public GameObject maze;
	public int currentZombieAmount = 0;
	public int currentSkeletonAmount = 0;
	public int currentMonsterAmount = 0;
	public int currentGolemAmount = 0;
	public bool playerIsInMaze = false;

	public PhotonView photonView;

	//private List<MazeRoom> listOfRooms;

	private int count = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (PhotonNetwork.isMasterClient) {
			if (playerIsInMaze) {
				if (count >= 100) {
//					Debug.Log (player.gameObject.transform.position);
//					Debug.Log (getMazeCoordsFromWorldCoords (player.gameObject.transform.position).x);
//					Debug.Log (getMazeCoordsFromWorldCoords (player.gameObject.transform.position).z);
					players = GameObject.FindGameObjectsWithTag("Player");

					List<MazeRoom> listOfRooms = new List<MazeRoom>();
					List<MazeRoom> listOfRoomsPlusCurrRoom = new List<MazeRoom>();
//					Debug.Log ("Players number: " + players.Length);
					foreach (GameObject player in players) { 
						MazeCell myCell = maze.GetComponent<Maze> ().GetCell (getMazeCoordsFromWorldCoords (player.gameObject.transform.position));
						if (myCell != null)	{
							listOfRooms = listOfRooms.Union<MazeRoom>(maze.GetComponent<Maze> ().getAdjacentRooms (myCell)).ToList();
							listOfRoomsPlusCurrRoom = listOfRoomsPlusCurrRoom.Union<MazeRoom> ( maze.GetComponent<Maze> ().getAdjacentRooms (myCell)).ToList();
							listOfRoomsPlusCurrRoom.Add (myCell.room);
						}
					}

					foreach (GameObject player in players) { 
						MazeCell myCell = maze.GetComponent<Maze> ().GetCell (getMazeCoordsFromWorldCoords (player.gameObject.transform.position));
						if (myCell != null)	{
							if (listOfRooms.Contains(myCell.room)) {
								listOfRooms.Remove(myCell.room);
							}
						}
					}

					updateEnemyCount (listOfRoomsPlusCurrRoom);
					count = 0;
					if (currentZombieAmount < maxZombieAmount) {
						MazeRoom randRoom = getRandomRoom (listOfRooms);
						MazeCell randCell = randRoom.getRandomCell ();
						Vector3 randPos = getWorldCoordsFromMazeCoords (randCell.coordinates);
						PhotonNetwork.InstantiateSceneObject ("Zombie", randPos, Quaternion.identity, 0, null);
						Debug.Log ("Zombie Spawn!");
						currentZombieAmount++;
					}
					if (currentSkeletonAmount < maxSkeletonAmount) {
						MazeRoom randRoom = getRandomRoom (listOfRooms);
						MazeCell randCell = randRoom.getRandomCell ();
						Vector3 randPos = getWorldCoordsFromMazeCoords (randCell.coordinates);
						PhotonNetwork.InstantiateSceneObject ("Skeleton", randPos, Quaternion.identity, 0, null);
						currentSkeletonAmount++;
						Debug.Log ("Skele Spawn!");

					}
					if (currentMonsterAmount < maxMonsterAmount) {
						MazeRoom randRoom = getRandomRoom (listOfRooms);
						MazeCell randCell = randRoom.getRandomCell ();
						Vector3 randPos = getWorldCoordsFromMazeCoords (randCell.coordinates);
						PhotonNetwork.InstantiateSceneObject ("Monster", randPos, Quaternion.identity, 0, null);
						currentMonsterAmount++;
						Debug.Log ("Monstaaaa Spawn!");
					}
					if (currentGolemAmount < maxGolemAmount) {
						MazeRoom randRoom = getRandomRoom (listOfRooms);
						MazeCell randCell = randRoom.getRandomCell ();
						Vector3 randPos = getWorldCoordsFromMazeCoords (randCell.coordinates);
						PhotonNetwork.InstantiateSceneObject ("Golem", randPos, Quaternion.identity, 0, null);
						currentGolemAmount++;
						Debug.Log ("YOU SHOULDNT BE Spawned!");
					}
				}
				count += 1;
			}
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
			IntVector2 currEnemyPos = getMazeCoordsFromWorldCoords(currEnemy.transform.position);
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
	public void broadcastPlayerEntersMaze(){
		photonView.RPC ("PlayerEnterMaze", PhotonTargets.MasterClient);
	}

	public void playerExitsMaze(){
//		playerIsInMaze = false;
	}

	[PunRPC]
	void PlayerEnterMaze() {
		playerIsInMaze = true;
	}
	public IntVector2 getMazeCoordsFromWorldCoords(Vector3 pos){
		int mazeX = (int)(pos.x/4f + 19.5f);
		int mazeZ = (int)(pos.z/4f + 19.5f);
		return new IntVector2 (mazeX, mazeZ);
	}
	public Vector3 getWorldCoordsFromMazeCoords(IntVector2 pos){
		float worldX = ((float)pos.x - 19.5f)*4f;
		float worldZ = ((float)pos.z - 19.5f)*4f;
		return new Vector3 (worldX, 9, worldZ);
	}
}
