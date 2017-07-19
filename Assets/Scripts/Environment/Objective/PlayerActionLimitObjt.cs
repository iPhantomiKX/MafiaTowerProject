using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionLimitObjt : Objective {
    public bool limitMelee;
    public bool limitGun;
    public bool failed = false;
    public override void onFail()
    {
        failed = true;
        this.enabled = false;
        om.OnFail(this.gameObject);
    }

    // Use this for initialization
    public override void Start () {
        failed = false;
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}
    public void NotifyGun()
    {
        if(limitGun)
        {
            onFail();
        }
    }
    public void NotifyMelee()
    {
        if (limitMelee)
        {
            onFail();
        }
    }
}
