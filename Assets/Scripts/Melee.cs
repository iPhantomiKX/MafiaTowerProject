using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour {

	SliderJoint2D sliderJoint;
	JointMotor2D jointMotor;
	CircleCollider2D collider;
	private AudioSource source;
	public AudioClip stabEnemySound;
	public AudioClip stabAirSound;

	void Awake(){
		sliderJoint = this.gameObject.GetComponent<SliderJoint2D>();
		jointMotor = sliderJoint.motor;
		collider = this.gameObject.GetComponent<CircleCollider2D> ();
		source = this.gameObject.transform.parent.GetComponent<AudioSource> ();
	}

	void Start () {
	}

	void Update () {
		if (PlayerController.meleeButton) {
			source.PlayOneShot (stabAirSound , stabAirSound.length);
			collider.enabled = true;
            if(FindObjectOfType<PlayerActionLimitObjt>() != null)
                 FindObjectOfType<PlayerActionLimitObjt>().NotifyMelee();
            collider.enabled = true;
			SetSpeedSlider (2f);
			this.transform.rotation = Quaternion.Euler(0,0, -90);
		} else {
			if (sliderJoint.jointTranslation >= sliderJoint.limits.max) {
				collider.enabled = false;
				SetSpeedSlider (-2f);
			}
		}
	}

	void SetSpeedSlider(float speed){
		jointMotor.motorSpeed = speed;
		sliderJoint.motor = jointMotor;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Enemy") {
			source.PlayOneShot (stabEnemySound , stabEnemySound.length);
			collider.enabled = false;
			EnemySM enem = col.gameObject.GetComponent<EnemySM> ();
			enem.TakeDamage (2f);
		}
	}

}