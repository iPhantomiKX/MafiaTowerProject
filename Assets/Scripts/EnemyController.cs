using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public GameObject player;
	public GameObject point1;
	public GameObject point2;
	GameObject target;

	public float radius;

	void Start(){
		target = point1;
	}

	void Update () {
		UpdateEnemyMovement ();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.Equals (point1))
			target = point2;
		else if (coll.gameObject.Equals (point2))
			target = point1;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (this.transform.position, radius);
	}

	void UpdateEnemyMovement()
	{
		float distance = Vector2.Distance (this.transform.position, player.transform.position);
		if (distance < radius) {
			Vector2 enemyPosition = this.transform.position;
			Vector2 playerPosition = player.transform.position;
			this.transform.position = Vector2.MoveTowards (this.transform.position, playerPosition, 1f * Time.deltaTime);
		} else {
			UpdateWalking ();
		}
	}

	void UpdateWalking(){
		this.transform.position = Vector2.MoveTowards (this.transform.position, target.transform.position, 0.7f * Time.deltaTime);
	}
}
