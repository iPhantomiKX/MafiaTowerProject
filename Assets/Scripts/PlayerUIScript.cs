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
	public Text currentAmmo;
	public Text maxAmmo;
	public Text healthText;


	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("PlayerObject");
        gun = player.GetComponentInChildren<Gun>();
        hc = player.GetComponent<HealthComponent>();

		maxAmmo.text = gun.ammo + "";
		healthSlider.maxValue = hc.health;
		UpdateText ();
	}
	
	// Update is called once per frame
	void Update () {
		healthSlider.value = hc.health;
		UpdateText ();
	}

	void UpdateText(){
		currentAmmo.text = gun.ammo + "";
		healthText.text = (healthSlider.value/healthSlider.maxValue)*100 + "%";
	}
}
