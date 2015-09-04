using UnityEngine;
using System.Collections;

public class ZombieMovement : Pathfinding {

	public float moveSpeed = 1f;
	public float rotateSpeed = 2f;
	public float vision = 2f;
	Transform target;
	GameObject player;
	Animator anim;
	public bool playerContact = false;
	public bool playerObserved = false;
	bool observeAudioPlayed = false;

	private bool pathMover = true;
	private bool newPath = true;
	public Vector3 start = Vector3.zero;
	public Vector3 end = Vector3.zero;

	AudioSource zombieAudio;
	
	void OnTriggerStay (Collider other)
	{	
		if(other.gameObject == player)
		{
			playerContact = true;
		}
	}

	void OnTriggerEnter (Collider other)
	{	
		if(other.gameObject == player)
		{
			playerContact = true;
		}
	}
	
	
	void OnTriggerExit (Collider other)
	{
		if(other.gameObject == player)
		{
			playerContact = false;
		}
	}


	void observePlayer() {

		float distance = Vector3.Distance (target.position, transform.position);

		if (distance <= vision) {
			playerObserved = true;
			if(anim){
				anim.SetBool ("ObservedPlayer", playerObserved);
			} else if(playerContact == false){
				GetComponent<Animation>().Play ("walk");
			}
			if (!observeAudioPlayed) {
				zombieAudio.Play ();
				observeAudioPlayed = true;
			}
		}

	}

	
	// Use this for initialization
	void Awake () {
		zombieAudio = GetComponent <AudioSource> ();
		anim = GetComponent <Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		target = player.transform;
		//anim.SetBool ("ObservedPlayer", true);
		start = transform.position;

	}

	IEnumerator PathTimer()
	{
		newPath = false;
		FindPath(transform.position, end);
		yield return new WaitForSeconds(0.5F);
		newPath = true;
	}

	private void Movement()
	{
		if (Path.Count > 0)
		{
			
			if (pathMover)
			{
				//StartCoroutine(PathRemoval(4F + 2F));
			}
			
			if (Vector3.Distance(transform.position, new Vector3(Path[0].x, transform.position.y, Path[0].z)) < 0.2F)
			{
				Path.RemoveAt(0);
			}
			
			
			if (Path.Count > 0)
			{             

				float rotateStep = rotateSpeed * Time.deltaTime;

				Vector3 direction = (new Vector3(Path[0].x, transform.position.y, Path[0].z) - transform.position).normalized;
				if (direction == Vector3.zero)
				{
					// direction = (end - transform.position).normalized;
				}
				transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * moveSpeed);
				Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotateStep, 0.0F);
				transform.rotation = Quaternion.LookRotation(newDir);

			}
		}
	}
	
	IEnumerator PathRemoval(float speed)
	{
		pathMover = false;
		yield return new WaitForSeconds((1 * Pathfinder.Instance.Tilesize) / speed);
		if (Path.Count > 0)
		{
			Path.RemoveAt(0);
		}
		pathMover= true;
	}
	
	// Update is called once per frame
	void Update () {
		bool isDead = GetComponent<ZombieHealth>().isDead;
		if(!isDead){

			observePlayer ();
			if(playerObserved){
				end = target.position;
				if (start != Vector3.zero && end != Vector3.zero && newPath)
				{
					StartCoroutine(PathTimer());
				}
				
				if (!playerContact) {
					Movement();
				} else {

				}
			};
		}

	}
}
