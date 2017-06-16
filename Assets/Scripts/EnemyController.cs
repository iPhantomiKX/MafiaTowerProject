﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public GameObject player;
	public float radius;

	void Update () {
		UpdateEnemyMovement ();
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (this.transform.position, radius);
	}

	void UpdateEnemyMovement()
	{
		float distance = Vector2.Distance (this.transform.position, player.transform.position);
		if (distance < radius) 
		{
			Vector2 enemyPosition = this.transform.position;
			Vector2 playerPosition = player.transform.position;
			this.transform.position = Vector2.MoveTowards (this.transform.position, playerPosition, 1f * Time.deltaTime);
		}
	}

	void OnCollisionEnter2D(Collision2D otherCollider)
	{
		if (otherCollider.gameObject.tag == "Player") 
		{
			player.GetComponent<HealthComponent>().TakeDmg(1);
		}
	}
}
