using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelInTime : Objective {

    public override bool check()
    {
        return !failed;
    }

    public override void onFail()
    {
        Debug.Log("Mandatory: " + mandatory);
        om.OnFail(this.gameObject);

        //this.enabled = false;

    }

    // Use this for initialization
    public override void Start () {
        objtname = "Finish this floor in " + time + " seconds!";
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        if (complete) om.OnComplete(gameObject);
        base.Update();
	}
}
