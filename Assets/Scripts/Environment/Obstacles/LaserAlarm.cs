using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAlarm : TraitObstacle {

	private AudioSource source;
	public AudioClip alarmSound;

	void Awake(){
		source = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
			source.PlayOneShot (alarmSound , alarmSound.length);
            GetComponent<EmitSound>().emitSound();
        }
    }

    public void SwitchOff()
    {
        gameObject.SetActive(false);
    }
}
