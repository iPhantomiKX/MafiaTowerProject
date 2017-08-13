using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockInspect : Inspect {

	private AudioSource source;
	public AudioClip knockingSound;

	void Awake(){
		source = GetComponent<AudioSource> ();

	}

    public override void inspect()
    {
		source.PlayOneShot (knockingSound);
        GetComponent<EmitSound>().emitSound();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
