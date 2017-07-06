﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEnemy : Objective
{
	public EnemySM[] enemies;
    public bool all;
    // Use this for initialization
    public override void Start()
    {
        objtname = "Defeat enemies";
        if(all)
        {
			enemies = GameObject.FindObjectsOfType<EnemySM>();
        }
        base.Start();

    }

    //// Update is called once per frame
    public override void Update()
    {
        int killed = 0;
        foreach(EnemySM enemy in enemies)
        {
            if (enemy.GetComponent<HealthComponent>().health <= 0) killed++;
        }
        numCompleted = killed;
        base.Update();
    }

    public override void onFail()
    {
        this.enabled = false;
        om.OnFail(this.gameObject);
    }

}
