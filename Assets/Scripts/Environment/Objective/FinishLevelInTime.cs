using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelInTime : Objective {
    public override void onFail()
    {
        this.enabled = false;
        om.OnFail(this.gameObject);

    }

    // Use this for initialization
    public override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        if (complete) om.OnComplete(gameObject);
        base.Update();
	}
}
