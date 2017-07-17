using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

	private Animator animator;

	// Background Sound
	private AudioSource bkSource;
	public AudioClip bkPeacefulSound;
	//public AudioClip bkBattleSound;

	// Sound Effect
	private AudioSource source;
	public AudioClip shootingSound;
	public AudioClip outOfAmmoSound;
	public AudioClip meleeSound;
	public AudioClip dyingSound;


	void Awake(){
		animator = this.GetComponent<Animator> ();
		source = GetComponent<AudioSource> ();
		bkSource = this.gameObject.transform.parent.GetComponent<AudioSource> ();
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
		animator.SetFloat ("speed", GetComponent<Rigidbody2D>().velocity.magnitude);

		// Shooting part
		if (PlayerController.shootButton && this.GetComponentInChildren<Gun> ().ammo > 0) {
			animator.Play ("PlayerShooting");
			source.PlayOneShot (shootingSound,shootingSound.length);
		}else if(PlayerController.shootButton){
			source.PlayOneShot (outOfAmmoSound, outOfAmmoSound.length);
		}

		// Melee part
		if (PlayerController.meleeButton) {
			animator.Play ("PlayerStabbing");
		}
	}

	void DyingSound(){
		source.PlayOneShot (dyingSound, meleeSound.length);
	}

}
