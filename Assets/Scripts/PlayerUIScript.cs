using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour {

	private HealthComponent hc;
	private Gun gun;
	public Slider ammoSlider;
	public Slider healthSlider;

	void Awake(){
		gun = this.gameObject.GetComponentInChildren<Gun>();
		hc = this.gameObject.GetComponent<HealthComponent>();
	}

	// Use this for initialization
	void Start () {
		ammoSlider.maxValue = gun.ammo;
		healthSlider.maxValue = hc.health;
	}
	
	// Update is called once per frame
	void Update () {
		ammoSlider.value = gun.ammo;
		healthSlider.value = hc.health;
	}


}
