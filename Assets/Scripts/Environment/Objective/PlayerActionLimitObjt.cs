﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionLimitObjt : Objective {
    public bool limitMelee;
    public bool limitGun;
    public override void onFail()
    {
        failed = true;
        om.OnFail(this.gameObject);
    }

    // Use this for initialization
    public override void Start () {
        failed = false;
        objtname = "Finish this floor without using ";
        if (limitMelee && limitGun) objtname += "any weapons!";
        else if (limitGun) objtname += "your gun!";
        else if (limitMelee) objtname += "your melee weapon!";
        
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

    public override bool check()
    {
        return !failed;
    }
}
