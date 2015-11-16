using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhotonNetworkManager : Photon.PunBehaviour {
	public Maze mazePrefab;
	private Maze mazeInstance;
	// Use this for initialization
	
	private Vector2 scrollPosition;
	//Declare String for room lobby names
	private string roomName = "Room01";
	private string playerName = "";
	//Declare for room status.....
	private string roomStatus = "";
	
	//Variables for max amount of players 0-20 "you can change this inside gui"
	private int maxPlayer =1;
	private string maxPlayerString = "1";

	//	private List<string> options = new List<string> ();
	//	static Rect position = new Rect(10, 10, 400, 100);
	//
	//	int selected = 2;
	//
	//	private bool test = false;
	
	public Font leagueGothicFont;
	public PhotonView photonView;
	
	void Start () {
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}
	
	void OnGUI () {
		GUIStyle lobbyLabelStyle = new GUIStyle();
		
		lobbyLabelStyle.fontSize = 50;
		lobbyLabelStyle.font = leagueGothicFont;
		lobbyLabelStyle.normal.textColor = Color.white;
		
		GUIStyle secondaryStyle = new GUIStyle();
		
		secondaryStyle.fontSize = 20;
		secondaryStyle.font = leagueGothicFont;
		secondaryStyle.normal.textColor = Color.white;
		
		//Show Detail of connection to master server
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		
		//GUILayout.Label (PhotonNetwork.GetPing ().ToString ());
		
		//Connection to master server lobby if joined
		//		if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
		//		{
		//			
		//			// Assign the it player to the appropriate player ID
		//			shoutMarco = GameLogic.playerWhoIsIt == PhotonNetwork.player.ID;
		//			
		//			//If I'm tagged then allow to say marco and send that threw an rpc using the method "Marco" to all targets with network view.
		//			if (shoutMarco && GUILayout.Button("Marco!"))
		//			{
		//				this.myPhotonView.RPC("Marco",PhotonTargets.All);
		//			}
		//			//I'f im not tagged then I can now say POLO to all network view.. with method RpC "Polo"
		//			if (!shoutMarco && GUILayout.Button("Polo"))
		//			{
		//				this.myPhotonView.RPC("Polo",PhotonTargets.All);
		//			}
		//		}
		//		
		
		//If I'm connected and inside lobby
		if (PhotonNetwork.insideLobby == true)
		{
			//Display the lobby connection list and room creation.
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0,0,Color.black);
			texture.Apply();
			GUI.skin.box.normal.background = texture;
			GUI.Box(new Rect(Screen.width/2f-250, Screen.height/2 - 250, 500, 500), "");
			GUILayout.BeginArea(new Rect(Screen.width/2-240, Screen.height/2 - 240, 480, 480));
			
			GUILayout.Box("Game Lobby", lobbyLabelStyle);
			GUI.color = Color.white;

			GUILayout.Label("Player Name:", secondaryStyle);
			playerName = GUILayout.TextField(playerName);

			// Ask for room name;
			GUILayout.Label("Room Name:", secondaryStyle);
			roomName = GUILayout.TextField(roomName);
			
			if (GUILayout.Button("Create Room") ){
				// Create random seed paramenter
				string[] roomPropsInLobby = {"randomSeed"};
				// Pre assign random seed variable with a random value. This value will be shared to every player in the same room.
				ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable () {{"randomSeed", Random.Range(1,9999999)}};
				
				RoomOptions customRoomOptions = new RoomOptions ();
				customRoomOptions.customRoomProperties = customRoomProperties;
				customRoomOptions.customRoomPropertiesForLobby = roomPropsInLobby;
				customRoomOptions.maxPlayers = 5;
				
				if (roomName != "" && maxPlayer > 0) // if the room name has a name and max players are larger then 0
				{
					PhotonNetwork.CreateRoom(roomName, customRoomOptions, TypedLobby.Default);
				}
			}
			
			GUILayout.Space(20);
			GUILayout.Box("Game Rooms Available", lobbyLabelStyle);
			GUI.color = Color.white;
			GUILayout.Space(20);
			
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, false ,true, GUILayout.Width(480), GUILayout.Height(300));
			
			foreach ( RoomInfo game in PhotonNetwork.GetRoomList()) // Each RoomInfo "game" in the amount of games created "rooms" display the fallowing.
			{
				
				GUI.color = Color.green;
				GUILayout.Box(game.name + " " + game.playerCount + "/" + game.maxPlayers); //Thus we are in a for loop of games rooms display the game.name provide assigned above, playercount, and max players provided. EX 2/20
				GUI.color = Color.white;
				
				if (GUILayout.Button("Join Room") ){
					
					PhotonNetwork.JoinRoom(game.name); // Next to each room there is a button to join the listed game.name in the current loop.
				}
			}
			
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
	}
	
	
	public override void OnConnectedToMaster ()
	{
		PhotonNetwork.JoinLobby ();
	}
	
	public override void OnJoinedLobby ()
	{
		PhotonNetwork.autoCleanUpPlayerObjects = true;

		//		//		RoomOptions roomOptions = new RoomOptions () {isVisible = true, maxPlayers = 10};
		//		string[] roomPropsInLobby = {"randomSeed"};
		//		ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable () {{"randomSeed", Random.Range(1,9999999)}};
		//		
		//		RoomOptions customRoomOptions = new RoomOptions ();
		//		customRoomOptions.customRoomProperties = customRoomProperties;
		//		customRoomOptions.customRoomPropertiesForLobby = roomPropsInLobby;
		//		PhotonNetwork.JoinOrCreateRoom("Main", customRoomOptions, TypedLobby.Default);
		
	}
	
	public override void OnJoinedRoom() {
		PhotonNetwork.playerName = playerName;
		photonView.RPC("newPlayerNameBroadcast", PhotonTargets.All, PhotonNetwork.playerName);
//		Debug.Log ("PLAYER: " + PhotonNetwork.playerName + " has joined the game");
//		Debug.Log ("Random Seed:" + PhotonNetwork.room.customProperties ["randomSeed"]);

		// Set Random Seed
		Random.seed = (int) PhotonNetwork.room.customProperties ["randomSeed"];
		StartCoroutine(StartGame());
		PhotonNetwork.player.TagObject = PhotonNetwork.Instantiate ("Player", new Vector3(-90f,2f,-70f), Quaternion.identity, 0);
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AudioListener> ().enabled = false;

		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.InstantiateSceneObject("TOD", new Vector3(897f, 70.05f, 372.357f), Quaternion.identity, 0, null);
		}
	}

	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{    
		Debug.Log ("Player Disconnected "+ player.name);
		photonView.RPC("playerLeaveGameBroadcast", PhotonTargets.All, player.name);
	}
	
	private IEnumerator StartGame() {
		Debug.Log ("Start Game");
		mazeInstance = Instantiate(mazePrefab) as Maze;
		yield return StartCoroutine(mazeInstance.Generate());
	}
	// Update is called once per frame
	void Update () {
	}
	
	
}
