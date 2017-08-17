using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatBossObjt : Objective {
    public BossData boss;

    public override bool check()
    {
        return complete;
    }

    public override void onFail()
    {
        om.OnFail(gameObject);
    }

    // Use this for initialization
    public override void Start () {
        objtname = "Defeat the boss of this level!";
        boss = FindObjectOfType<BossData>();
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        if(boss.GetComponent<HealthComponent>().health <= 0 && !complete)
        {
            complete = true;
            om.OnComplete(gameObject);
        }
        base.Update();
	}
}
