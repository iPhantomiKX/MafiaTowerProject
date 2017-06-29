using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour {

	SliderJoint2D sliderJoint;
	JointMotor2D jointMotor;

	void Awake(){
		sliderJoint = this.gameObject.GetComponent<SliderJoint2D>();
		jointMotor = sliderJoint.motor;
	}

	void Start () {
	}

	void Update () {
		if (PlayerController.meleeButton) {
			SetSpeedSlider (2f);
			this.transform.rotation = Quaternion.Euler(0,0, -90);
		} else {
			if (sliderJoint.jointTranslation >= sliderJoint.limits.max) {
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
			EnemySM enem = col.gameObject.GetComponent<EnemySM> ();
			enem.HP -= 2;
		}
	}

}