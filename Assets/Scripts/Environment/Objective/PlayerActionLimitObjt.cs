using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionLimitObjt : Objective {
    public bool limitMelee;
    public bool limitGun;
    public bool limitHeal;
    public override void onFail()
    {
        failed = true;
        om.OnFail(this.gameObject);
    }

    // Use this for initialization
    public override void Start () {
        failed = false;
        objtname = "Finish this floor without using ";
        if (limitMelee && limitGun && limitHeal) objtname += "any weapons or healing items!";
        else if (limitMelee && limitGun) objtname += "any weapons!";
        else if (limitMelee && limitHeal) objtname += "healing items and melee weapon!";
        else if (limitGun && limitHeal) objtname += "healing items and your gun!";
        else if (limitGun) objtname += "your gun!";
        else if (limitMelee) objtname += "your melee weapon!";
        else if (limitHeal) objtname += "healing items!";
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}
    public void NotifyGun()
    {
        if(limitGun && !failed)
        {
            onFail();
        }
    }
    public void NotifyMelee()
    {
        if (limitMelee && !failed)
        {
            onFail();
        }
    }
    public void NotifyHeal()
    {
        if (limitHeal && !failed) onFail();
    }

    public override bool check()
    {
        return !failed;
    }
}
