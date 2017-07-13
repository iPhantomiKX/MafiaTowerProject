using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour {

	private HealthComponent hc;
	private Gun gun;
	public Slider ammoSlider;
	public Slider healthSlider;
    public GameObject player;

	// Use this for initialization
	void Start ()
    {
        Debug.Log("test");
        player = GameObject.Find("PlayerObject");
        gun = player.GetComponentInChildren<Gun>();
        hc = player.GetComponent<HealthComponent>();

        ammoSlider.maxValue = gun.ammo;
		healthSlider.maxValue = hc.health;
	}
	
	// Update is called once per frame
	void Update () {
		ammoSlider.value = gun.ammo;
		healthSlider.value = hc.health;
	}


}
