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
		if (PlayerController.shootButton && this.GetComponentInChildren<Gun>().ammo > 0) {
			animator.Play ("PlayerShooting");
		}

	}
}
