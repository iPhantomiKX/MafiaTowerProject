using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public GameObject player;
	private Rigidbody2D rb;
	private float timeToChangeDirection;

	public float radius;
	public float angleFOV;
	public float dmgCooldown;

	private float dmgCooldownCountdown = 0.0f;
	private bool PlayerContact = false;


	private Vector3 LastPlayerPosition;
	public Vector3 SuspiciousPosition{ get; set; }
	public float SuspiciousTime{ get; set; }

    private GameStateManager GameStateRef;

	void Start(){
		SuspiciousPosition = Vector3.forward;
		SuspiciousTime = 0f;
	}

    void Update()
    {
        //Debug.Log (dmgCooldownCountdown);

        if (!GameStateRef)
            GameStateRef = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();

        if (GameStateRef.CurrentState == GameStateManager.GAME_STATE.RUNNING)
        {
            if (rb.IsSleeping())
                rb.WakeUp();

            if (isWalking())
            {
                enemyWalk();
            }
            else
            {
                catchPlayer();
            }
        }
        else
        {
            rb.Sleep();
        }
    }

	private void Awake(){
		rb = GetComponent<Rigidbody2D>();
	}

	bool isWalking(){
		/**float distance = Vector2.Distance (this.transform.position, player.transform.position);
		if (distance < radius)
			return false;
		else
			return true;
			*/

		Vector3 playerDir = player.transform.position - this.transform.position;
		Vector3 forward = this.transform.up;
		float angle = Vector3.Angle (playerDir, forward);
		float distance = Vector3.Distance (player.transform.position, this.transform.position);


		if(SuspiciousPosition != Vector3.forward){
			SuspiciousTime -= Time.deltaTime;
			if (SuspiciousTime <= 0)
				stopSuspicious();
			return false;
		} 

		if (angle < angleFOV && distance < radius) {
			int layerMask = 1 << 8;
			RaycastHit2D hit = Physics2D.Raycast (this.transform.position, playerDir,Mathf.Infinity,layerMask);

			if (hit.collider != null) {
				if (hit.collider.gameObject.tag == "Player") {
					return false;


				}
			} else {
				//In case part of the body is seen
				RaycastHit2D hit2 = Physics2D.Raycast (this.transform.position, Quaternion.AngleAxis(playerDir.z + 10f, Vector3.forward) * playerDir,Mathf.Infinity,layerMask);

				if (hit2.collider != null) {
					if (hit2.collider.gameObject.tag == "Player") {
						return false;


					}
				} else {
					RaycastHit2D hit3 = Physics2D.Raycast (this.transform.position, Quaternion.AngleAxis(playerDir.z - 10f, Vector3.forward) * playerDir,Mathf.Infinity,layerMask);

					if (hit3.collider != null) {
						if (hit3.collider.gameObject.tag == "Player") {
							return false;


						}
					} 
				}
			}

			return true;
		} else
			return true;






	}

	void stopSuspicious(){
		SuspiciousPosition = Vector3.forward;
		SuspiciousTime = 0;
	}

	void enemyWalk(){
		timeToChangeDirection -= Time.deltaTime;
		if (timeToChangeDirection <= 0)  {
			ChangeDirection ();
		}
		rb.velocity = transform.up * 0.5f;
	}

	void catchPlayer(){
		Vector2 playerPosition = player.transform.position;
		this.transform.position = Vector2.MoveTowards (this.transform.position, playerPosition, 1f * Time.deltaTime);
		Vector3 toTarget = player.transform.position - this.transform.position;
		float angle = (Mathf.Atan2 (toTarget.y, toTarget.x) * Mathf.Rad2Deg)-90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, q, Time.deltaTime * 5f);
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (this.transform.position, radius);
	}
		
	void OnCollisionStay2D(Collision2D otherCollider)
	{
		if (otherCollider.gameObject.tag == "Player") 
		{
			if (!PlayerContact) 
			{
				PlayerContact = true;
				player.GetComponent<HealthComponent>().TakeDmg(1);
				dmgCooldownCountdown = dmgCooldown;
			}
			if (PlayerContact) 
			{
				dmgCooldownCountdown -= 1.0f;
				if (dmgCooldownCountdown <= 0) {
					player.GetComponent<HealthComponent> ().TakeDmg (1);
					dmgCooldownCountdown = dmgCooldown;
				}
				else 
				{
					player.GetComponent<HealthComponent> ().TakeDmg (0);
				}
			}
		}
	}

	void ChangeDirection() {
		float angle = Random.Range(0f, 360f);
		Quaternion quat = Quaternion.AngleAxis(angle, Vector3.forward);
		Vector3 newUp = quat * Vector3.up;
		newUp.z = 0;
		newUp.Normalize ();
		this.transform.up = newUp;
		timeToChangeDirection = Random.Range(0f, 2.5f);
	}
}