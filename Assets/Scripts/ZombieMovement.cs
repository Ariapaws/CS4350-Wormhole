using UnityEngine;
using System.Collections.Generic;

public class ZombieMovement : MonoBehaviour
{

    public float moveSpeed = 1f;
    public float rotateSpeed = 2f;
    public float vision = 20f; // how far zombie can see within angle of vision
    public float nightMoveSpeed = 3f;
    public float nightRotateSpeed = 5f;
    public float nightVision = 5f;
    public float startingMoveSpeed = 1f;
    public float startingRotateSpeed = 2f;
    public float startingVision = 2f;
    public float angleOfVision = 45f; // i.e. zombie can see 45 degrees left and right 
    public float attentionTime = 5f;
    public float attentionCountdown = 0f;
    public float smell = 5f;
    Transform target = null;
    GameObject player = null;
    Animator anim;
    public bool playerContact = false;
    public bool playerObserved = false;
    bool observeAudioPlayed = false;

    AudioSource zombieAudio;

    // spherecast to look out for player
    float sphereThickness = 0.3f;
    float sphereRange = 20f; //vision of zombie
    Vector3 sphereOrigin;
    Vector3 sphereDirection;
    RaycastHit sphereHit;

    // raycast to look out for walls
    Ray shootRay;
    RaycastHit shootHit;

    Vector3[] sphereDirections;
    private float distanceFromTarget = Mathf.Infinity;
    private List<Vector3> playerTracks = new List<Vector3>(); //list of positions of player
    private int updateCount = 0;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            playerContact = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerContact = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerContact = false;
        }
    }

    void playAnimationAndAudio(float distanceFromTarget)
    {
        if (anim)
        {
            anim.SetBool("ObservedPlayer", playerObserved);
        }
        else if (playerContact == false) //distanceFromTarget > 2
        {
            //GetComponent<Animation>().Play("walk");
            anim.SetTrigger("Walk");
        }
        if (!observeAudioPlayed)
        {
            zombieAudio.Play();
            observeAudioPlayed = true;
        }
    }

    void observePlayer()
    {
        // Check if any players within immediate distance
        GameObject[] all_players = GameObject.FindGameObjectsWithTag("Player");
        float[] player_distances = new float[all_players.Length];
        for (int i = 0; i < all_players.Length; i++) {
            distanceFromTarget = Vector3.Distance(transform.position, all_players[i].transform.position);
            if (distanceFromTarget < smell)
            {
                player = all_players[i];
                target = player.transform;

                // use spherecast to check if player is in sight
                sphereOrigin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                //Vector3 playerDirection = new Vector3(target.position.x, transform.position.y, target.position.z); //keep to same height as zombie
                sphereDirection = (target.position - transform.position).normalized;

                bool isSphereCastHit = Physics.SphereCast(sphereOrigin, sphereThickness, sphereDirection, out sphereHit, smell);
                if (isSphereCastHit && sphereHit.transform.gameObject.tag == "Player")
                {
                    player = sphereHit.transform.gameObject;
                    target = player.transform;
                    GetComponent<ZombieAttack>().player = player;
                    GetComponent<ZombieAttack>().playerHealth = player.GetComponent<PlayerHealth>();
                    GetComponent<ZombieHealth>().player = player;
                    GetComponent<ZombieHealth>().PlayerAssets = player.GetComponent<PlayerAssets>();
                    playerObserved = true;
                    attentionCountdown = attentionTime;
                    playAnimationAndAudio(distanceFromTarget);
                    break;
                }
            }
        }
        if (!playerObserved)
        {
            for (int i = 0; i < all_players.Length; i++)
            {
                distanceFromTarget = Vector3.Distance(transform.position, all_players[i].transform.position);
                if (distanceFromTarget <= sphereRange)
                {
                    player = all_players[i];
                    target = player.transform;

                    // calculate angle to see if player is in sight, then
                    // use spherecast to check if player is in sight
                    sphereOrigin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                    //Vector3 playerPosWithSameHeight = new Vector3(target.position.x, transform.position.y + 0.5f, target.position.z); //keep to same height as zombie
                    sphereDirection = (target.position - transform.position).normalized;

                    if (Vector3.Angle(sphereDirection, transform.forward) <= angleOfVision) // only spherecast when within field of vision
                    {
                        bool isSphereCastHit = Physics.SphereCast(sphereOrigin, sphereThickness, sphereDirection, out sphereHit, sphereRange);
                        if (isSphereCastHit && sphereHit.transform.gameObject.tag == "Player")
                        {
                            player = sphereHit.transform.gameObject;
                            target = player.transform;
                            GetComponent<ZombieAttack>().player = player;
                            GetComponent<ZombieAttack>().playerHealth = player.GetComponent<PlayerHealth>();
                            GetComponent<ZombieHealth>().player = player;
                            GetComponent<ZombieHealth>().PlayerAssets = player.GetComponent<PlayerAssets>();
                            playerObserved = true;
                            attentionCountdown = attentionTime;
                            playAnimationAndAudio(distanceFromTarget);
                            break;
                        }

                    }
                }
            }
        }
    }

    void updatePlayerObserved()
    {
        if (attentionCountdown <= 0)
        {
            playerObserved = false;
        } else
        {
            // use spherecast to check if player is still in sight
            sphereOrigin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            //Vector3 playerPosWithSameHeight = new Vector3(target.position.x, transform.position.y + 0.5f, target.position.z); //keep to same height as zombie
            sphereDirection = (target.position - transform.position).normalized;

            bool isSphereCastHit = Physics.SphereCast(sphereOrigin, sphereThickness, sphereDirection, out sphereHit, sphereRange);
            if (isSphereCastHit && sphereHit.transform.gameObject == player)
            {
                // this means that playerObserved = true;
                attentionCountdown = attentionTime;
                playAnimationAndAudio(distanceFromTarget);
            } else
            {
                playerObserved = false;
                //player = null;  // note: commented out becauase if attentionCountdown > 0 when playerObserved == false,
                //target = null;  // should still try to track player
            }
        }
    }


    // Use this for initialization
    void Awake()
    {
        zombieAudio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        //player = GameObject.FindGameObjectWithTag("Player");
        //target = player.transform;
        //anim.SetBool ("ObservedPlayer", true);
        sphereRange = vision;
    }

    private void moveTowardsPlayer()
    {
        bool hasMoved = false;
        float rotateStep = rotateSpeed * Time.deltaTime;
        Vector3 newDir;

        if (playerTracks.Count > 0)
        {
            Vector3 currPoint;
            float distanceToPoint;
            for (int i = playerTracks.Count - 1; i >= 0; i--)
            {
                currPoint = playerTracks[i];

                // use spherecast to check if able to move to that spot
                sphereOrigin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                //Vector3 pointPosWithSameHeight = new Vector3(currPoint.x, transform.position.y + 0.5f, currPoint.z); //keep to same height as zombie
                sphereDirection = (currPoint - sphereOrigin).normalized;
                distanceToPoint = Vector3.Distance(currPoint, sphereOrigin);

                bool isSphereCastHit = Physics.SphereCast(sphereOrigin, 0.1f, sphereDirection, out sphereHit, distanceToPoint);
                /*
                if (isSphereCastHit)
                {
                    Debug.Log(distanceToPoint);
                    Debug.Log("spherecast hit: " + sphereHit.transform.gameObject.name+ " at "+Vector3.Distance(sphereHit.point,sphereOrigin).ToString()+" distance");
                    Debug.DrawLine(sphereOrigin, sphereHit.point,Color.red,5.0f);
                } else
                {
                    Debug.Log("spherecast didn't hit anything");
                }
                */
                if (!isSphereCastHit || (isSphereCastHit && sphereHit.transform.gameObject.name != "Wall"))
                {
                    //Debug.Log("Moving, playerTracks.Count: "+playerTracks.Count.ToString());
                    // remove older tracks and move towards point
                    if (playerTracks.Count > 10 && i < playerTracks.Count - 2)
                    {
                        playerTracks = playerTracks.GetRange(i, playerTracks.Count - i);
                    } else if (playerTracks.Count > 30)
                    {
                        playerTracks = playerTracks.GetRange(playerTracks.Count - 20, 20);
                    }
                    Vector3 directionToMove = new Vector3(currPoint.x, transform.position.y, currPoint.z) - transform.position;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + directionToMove, Time.deltaTime * moveSpeed);
                    newDir = Vector3.RotateTowards(transform.forward, directionToMove, rotateStep, 0.0F);
                    transform.rotation = Quaternion.LookRotation(newDir);
                    hasMoved = true;
                    break;
                }
            }
        }
        if (!hasMoved)
        {
            //Debug.Log("No player tracks to follow, stop animation.");
            anim.SetBool("ObservedPlayer", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        GameManager gameManagerScript = (GameManager)gameManagerObject.GetComponent(typeof(GameManager));
        bool isNight = gameManagerScript.isNight;

        if (isNight)
        {
            moveSpeed = nightMoveSpeed;
            rotateSpeed = nightRotateSpeed;
            vision = nightVision;
            sphereRange = vision;
        }
        else
        {
            moveSpeed = startingMoveSpeed;
            rotateSpeed = startingRotateSpeed;
            vision = startingVision;
            sphereRange = vision;
        }

        bool isDead = GetComponent<ZombieHealth>().isDead;
        if (!isDead)
        {
            attentionCountdown -= Time.deltaTime;
            if (attentionCountdown <= 0) { 
                attentionCountdown = 0;
                playerTracks = new List<Vector3>();// Clear playerTracks when attentionCountdown reaches 0.
                anim.SetBool("ObservedPlayer", false);
            } 
            if (target != null && attentionCountdown > 0)
            {
                if (playerTracks.Count == 0 || Vector3.Distance(playerTracks[playerTracks.Count - 1], target.position) > 0.1f)
                {
                    playerTracks.Add(target.position);
                    string tracksString = "";
                    for (int i=0; i<playerTracks.Count; i++)
                    {
                        tracksString += playerTracks[i].ToString()+",";
                    }
                    //Debug.Log("Added to playerTracks: " + tracksString);
                }
            }

            if (!playerObserved)
                observePlayer(); //spherecast to check if any player within range/sight, also plays animation and audio and starts attentionCountdown
            else
                updatePlayerObserved(); // if attentionCountdown > 0, spherecast to check if target player is within sight
            if (attentionCountdown > 0)
            {
                if (!playerContact)
                {
                    //Movement();
                    moveTowardsPlayer();
                }
            }
        }

    }
}
