using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

	private Animator animator;

	void Awake(){
		animator = this.GetComponent<Animator> ();

	}

	void Start () {
		animator.SetInteger ("health", this.gameObject.GetComponent<HealthComponent> ().health);
	}
		
	void Update () {
		animator.SetInteger ("health", this.gameObject.GetComponent<HealthComponent> ().health);
		if (animator.GetInteger ("health") > 0) {
			Alive ();
		}

	}

	void Alive(){
		animator.SetFloat ("speed", this.GetComponent<PlayerController> ().rb.velocity.magnitude);

		if (PlayerController.shootButton && this.GetComponentInChildren<Gun> ().ammo > 0) {
			animator.Play ("PlayerShooting");
		}else if (PlayerController.meleeButton) {
			animator.Play ("PlayerStabbing");
		}
	}
}
