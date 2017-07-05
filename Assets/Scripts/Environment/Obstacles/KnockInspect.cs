using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockInspect : Inspect {
    public override void inspect()
    {
        GetComponent<EmitSound>().emitSound();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
