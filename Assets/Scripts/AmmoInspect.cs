using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInspect : Inspect {

	public int collectedAmmo;
	private AudioSource source;
	public AudioClip collectingSound;

	void Awake(){
		source = GetComponent<AudioSource>();
	}
	public override void inspect()
	{
		source.PlayOneShot (collectingSound , collectingSound.length);
		GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Gun>().CollectedAmmo(collectedAmmo);
		GetComponent<CircleCollider2D> ().enabled = false;
		GetComponent<SpriteRenderer> ().enabled = false;
		//Destroy (this.gameObject);
	}

}
