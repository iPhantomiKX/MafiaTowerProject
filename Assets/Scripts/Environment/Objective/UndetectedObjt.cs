using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndetectedObjt : Objective {
    public override bool check()
    {
        return !failed;
    }

    public override void onFail()
    {
        failed = true;
        om.OnFail(this.gameObject);
    }

    // Use this for initialization
    public override void Start () {
        objtname = "Finish this floor undetected!";
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        if (!failed)
        {
            EnemySM[] enemies = FindObjectsOfType<EnemySM>();
            foreach (EnemySM enemy in enemies)
            {
                if (enemy.alert == true) onFail();
            }
        }
        base.Update();
	}
}
