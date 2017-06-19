using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public GameObject player;
	private Rigidbody2D rb;
	private float timeToChangeDirection;

	public float radius;
	public float dmgCooldown;

	private float dmgCooldownCountdown = 0.0f;
	private bool PlayerContact = false;

	void Start(){
		
	}

	void Update () {
		//Debug.Log (dmgCooldownCountdown);

		if (isWalking ()) {
			enemyWalk ();
		} else {
			catchPlayer ();
		}
			
	}

	private void Awake(){
		rb = GetComponent<Rigidbody2D>();
	}

	bool isWalking(){
		float distance = Vector2.Distance (this.transform.position, player.transform.position);
		if (distance < radius)
			return false;
		else
			return true;
	}

	void enemyWalk(){
		timeToChangeDirection -= Time.deltaTime;
		if (timeToChangeDirection <= 0) {
			ChangeDirection ();
		}
		rb.velocity = transform.up * 0.5f;
	}

	void catchPlayer(){
		Vector2 playerPosition = player.transform.position;
		this.transform.position = Vector2.MoveTowards (this.transform.position, playerPosition, 1f * Time.deltaTime);
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
