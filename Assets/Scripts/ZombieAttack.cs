using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZombieAttack : MonoBehaviour {

	public float timeBetweenAttacks = 0.5f;
    public float ratioOftimeBeforeDamage = 0.4f;
    public float ratioOfTimeForDamage = 0.2f;
    public float durationOfAnimation = 2.0f;
	public int attackDamage = 10;
	public int startingDamage = 10;
	public int nightDamage = 50;
	public AudioClip attackClip;


	public Animator anim;
	public GameObject player;
	public PlayerHealth playerHealth;
	public AudioSource zombieAudio;
	public ZombieHealth zombieHealth;
	public bool playerInRange;
	public float cooldownTimer;
    private bool isCooldownTimerOn = true;
    public float damageTimer;
    private bool isDamageTimerOn = false;
    private bool hasAttacked = false;
	public bool isAttacking;

	public Text feedback;
	
	void Awake ()
	{
       // player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerHealth> ();
		zombieAudio = GetComponent <AudioSource> ();
		zombieHealth = GetComponent<ZombieHealth>();
		anim = GetComponent <Animator> ();
		GameObject feedbackObject = GameObject.FindGameObjectWithTag("Feedback");
		feedback = feedbackObject.GetComponent<Text>();
	}
	
	
	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject == player)
		{
			playerInRange = true;
		}
	}

	void OnTriggerStay(Collider other){
		if(other.gameObject == player)
		{
			playerInRange = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if(other.gameObject == player)
		{
			playerInRange = false;
		}
	}
	
	
	void Update ()
	{
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag ("GameManager");
		GameManager gameManagerScript = (GameManager)gameManagerObject.GetComponent(typeof(GameManager));
		bool isNight = gameManagerScript.isNight;

		if (isNight) {
			attackDamage = nightDamage;
		} else {
			attackDamage = startingDamage;
		}

        if (isCooldownTimerOn)
        {
            cooldownTimer += Time.deltaTime;
        }
        if (isDamageTimerOn)
        {
            damageTimer += Time.deltaTime;
        }
        if (isDamageTimerOn && damageTimer < durationOfAnimation * (ratioOftimeBeforeDamage + ratioOfTimeForDamage) && 
            damageTimer >= durationOfAnimation * ratioOftimeBeforeDamage && 
            playerInRange && zombieHealth.currentHealth > 0)
        {
            if (!hasAttacked)
            {
                hasAttacked = true;
                Attack();   
            }
        }
        if (isDamageTimerOn && damageTimer >= durationOfAnimation)
        {
            cooldownTimer = 0.0f;
            isCooldownTimerOn = true;
            damageTimer = 0.0f;
            isDamageTimerOn = false;
            hasAttacked = false;

        }
        if (isCooldownTimerOn && cooldownTimer >= timeBetweenAttacks && playerInRange && zombieHealth.currentHealth > 0)
		{
			if(anim){
				anim.SetTrigger ("Attack");
                isDamageTimerOn = true;
                isCooldownTimerOn = false;
			}
            /*else {
				GetComponent<Animation>().Play ("punch",PlayMode.StopSameLayer);
			}*/
		}
	}

	void Attack ()
	{
		if(playerHealth.currentHealth > 0)
		{
			zombieAudio.clip = attackClip;
			zombieAudio.Play ();
			Vector3 direction = (new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position).normalized;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, 2f, 0.0F);
			transform.rotation = Quaternion.LookRotation(newDir);

            if (playerHealth.armor >= attackDamage)
            {
                feedback.color = new Color(1, 1, 1, 1);
                feedback.text = "Damage blocked!";
            }
            else {
			    feedback.color = new Color(1,0,0,2);
			    feedback.text = "Damage received: "+attackDamage.ToString();
            }
			playerHealth.TakeDamage (attackDamage);

		}

	}

    /********************
       OLD CODE BELOW
    ********************/
    bool attackComplete()
    {
        if (this.isAttacking && !(anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit")))
        {
            this.isAttacking = false;
            Debug.Log("attack completed");
            return true;

        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // Avoid any reload.
            this.isAttacking = true;
            Debug.Log("attack in progress");
            return false;

        }
        else
        {
            this.isAttacking = false;
            Debug.Log("attack interrputed");
            return false;
        }
    }
}
