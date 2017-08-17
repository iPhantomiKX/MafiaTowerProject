using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAlarm : TraitObstacle {

	private AudioSource source;
	public AudioClip alarmSound;
    float cooldown = 3;
    float cooldownLeft;
    
	void Awake(){
		source = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
        cooldownLeft = cooldown;
	}
	
	// Update is called once per frame
	void Update () {

        base.Update();

        if (cooldownLeft <= 0) cooldownLeft = 0;
        else cooldownLeft -= Time.deltaTime;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player" && cooldownLeft <= 0)
        {
			source.PlayOneShot (alarmSound , alarmSound.length);
            GetComponent<EmitSound>().emitSound();
            cooldownLeft = cooldown;
        }
    }

    public void SwitchOff()
    {
        gameObject.SetActive(false);
    }
}
