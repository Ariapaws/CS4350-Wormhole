using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{

    /**************** Start of variables to be changed ********************************/

    //attack range
    public float range = 5f;

    // attack speed (for animation...not sure does it really kill faster)
    public float attackSpeed = 1f;

    // base damage
    public int damagePerHit = 20;

    // defense
    public int defense = 5;

    // stamina used for blocking
    public float blockStaminaReduction = 10;

    /**************** End of variables to be changed ********************************/

    public GameObject target;

    int attacknumber = 0;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    AudioSource playerAudio;
    public Animator animator;
    public AudioClip hitClip;
    public AudioClip swingClip;
    public bool attackPerforming = false;
    public bool block = false;
    public string[] attackName;
    public string currentAttack;
    public Animation animation;

    // For spherecast (torch)
    float sphereThickness = 0.4f;
    float sphereRange = 2.3f;
    Vector3 sphereOrigin;
    Vector3 sphereDirection;
    RaycastHit sphereHit;

    // Use this for initialization
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        shootableMask = LayerMask.GetMask("Shootable");
        animator = GetComponentInChildren<Animator>();
        attackName = new string[] { "Attack", "Attack1", "Attack2" };
        currentAttack = "Attack";
        animation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && Input.GetAxis("Fire1") <= 0)
        {
            if (animator.GetInteger("Block") == 0)
            {
                animator.SetInteger("Block", 1);
            }

            /*
			//block animation enabled
			animator.SetBool ("Guard", true);
             */
            GetComponent<PlayerHealth>().armor += defense;
            block = true;

        }
        else if (Input.GetMouseButtonUp(1))
        {
            animator.SetInteger("Block", 0);
            /*
			//block animation disabled
			animator.SetBool("Guard", false);
             * */
            block = false;
            GetComponent<PlayerHealth>().armor -= defense;
        }

      // if attacking without blocking
        else if (Input.GetAxis("Fire1") > 0 && !Input.GetMouseButtonDown(1))
        {
            // attack animation enabled
            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            // For spherecast (torch)
            sphereOrigin = transform.position;
            sphereDirection = transform.TransformDirection(Vector3.forward);
            StartCoroutine(attack());

            //block animation disabled
            //animator.SetBool("Guard", false);
            animator.SetInteger("Block", 0);
            block = false;
            GetComponent<PlayerHealth>().armor -= defense;

        }

        // if attacking and blocking
        else if (Input.GetAxis("Fire1") > 0 && Input.GetMouseButtonDown(1))
        {
            //block animation enabled
            animator.SetInteger("Block", 1);
            //animator.SetBool("Guard", true);
            GetComponent<PlayerHealth>().armor += defense;
            block = true;
        }

        reduceStaminaForBlocking(block);

    }

    void reduceStaminaForBlocking(bool block)
    {
        if (block)
        {
            //reduce stamina over time
            GetComponent<PlayerHealth>().ReduceStamina(blockStaminaReduction * Time.deltaTime);
        }
    }

    IEnumerator attack()
    {

        if (attackPerforming)
        {
            yield return new WaitForSeconds(0);

            // same attack name
            // changed to another name 
            if (string.Compare(currentAttack, attackName[0]) == 0)
            {
                currentAttack = attackName[1];
            }
            else if (string.Compare(currentAttack, attackName[1]) == 0)
            {
                currentAttack = attackName[2];
            }
            else if (string.Compare(currentAttack, attackName[2]) == 0)
            {
                currentAttack = attackName[0];
            }

        }
        else
        {

			GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, currentAttack);
            animator.SetTrigger(currentAttack);

            attackPerforming = true;
            bool isSphereCastHit = Physics.SphereCast(sphereOrigin, sphereThickness, sphereDirection, out sphereHit, sphereRange);
            if (Physics.Raycast(shootRay, out shootHit, range))
            {
                Debug.DrawLine(shootRay.origin, shootHit.point);
                ZombieHealth enemyHealth = shootHit.collider.GetComponent<ZombieHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerHit);
                    playerAudio.clip = hitClip;
                    playerAudio.Play();
                }
                else if (isSphereCastHit)
                {
					Debug.Log("DESTROY");
                    tryToHitTorch(sphereHit);
                }
                else
                {
                    playerAudio.clip = swingClip;
                    playerAudio.Play();
                }
            }
            else if (isSphereCastHit)
            {
				Debug.Log("DESTROY1");

                tryToHitTorch(sphereHit);
            }
            else
            {

                playerAudio.clip = swingClip;
                playerAudio.Play();
            }
            yield return new WaitForSeconds(0.5f);
            attackPerforming = false;
        }
    }

    void tryToHitTorch(RaycastHit sphereHit)
    {

        GameObject torchObject = null;
        Torch torchScript = sphereHit.collider.GetComponent<Torch>();
        if (torchScript != null)
        {
            torchObject = torchScript.gameObject;
        }
        if (torchObject != null)
        {
            playerAudio.clip = hitClip;
            playerAudio.Play();
            PhotonNetwork.Destroy(torchObject);
            PlayerAssets playerAssetsScript = GetComponent<PlayerAssets>();
            playerAssetsScript.numOfTorchesLeft++;
        }
        else
        {
            playerAudio.clip = swingClip;
            playerAudio.Play();
        }
    }
}
