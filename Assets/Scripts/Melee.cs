using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour {

	private float cooldown;

	void Start () {
		cooldown = 0.0f;
	}

	void Update () {

		if (PlayerController.meleeButton && cooldown == 0) {
			Attack ();
		} else {
			if (cooldown <= 0) {
				cooldown = 0.0f;
				SetAppear (false);
			} else {
				cooldown -= Time.deltaTime;
			}

		}

	}

	void SetAppear(bool boolean){
		this.gameObject.GetComponent<CircleCollider2D>().enabled = boolean;
		this.gameObject.GetComponent<SpriteRenderer>().enabled = boolean;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.name == "Enemy") {
			Destroy (col.gameObject);
		}
	}

	void Attack(){
		SetAppear (true);
		cooldown = 0.01f;
	}

}
