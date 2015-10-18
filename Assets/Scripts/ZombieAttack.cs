using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZombieAttack : MonoBehaviour {

	public float timeBetweenAttacks = 0.5f;
    public float ratioOftimeBeforeDamage = 0.4f;
    public float ratioOfTimeForDamage = 0.2f;
    public float durationOfAnimation = 2.0f;
	public int attackDamage = 10;
	public AudioClip attackClip;
	
	Animator anim;
	GameObject player;
	PlayerHealth playerHealth;
	AudioSource zombieAudio;
	ZombieHealth zombieHealth;
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
        // Note if need to find which player to get aggro
        // http://answers.unity3d.com/questions/598323/how-to-find-closest-object-with-tag-one-object.html
        player = GameObject.FindGameObjectWithTag ("Player");
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
			feedback.color = new Color(1,0,0,2);
			feedback.text = "Damage received: "+attackDamage.ToString();
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
