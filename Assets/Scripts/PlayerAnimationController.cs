using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

	private Animator animator;

	void Awake(){
		animator = this.GetComponent<Animator> ();
	}

	void Start () {
	}

	void Update () {
		
		animator.SetFloat ("speed", this.GetComponent<PlayerController> ().rb.velocity.magnitude);

		if (PlayerController.shootButton && this.GetComponentInChildren<Gun>().ammo > 0) {
			animator.Play ("PlayerShooting");
		}

	}
}
