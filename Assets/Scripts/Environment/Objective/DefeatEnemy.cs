using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEnemy : Objective
{
	public EnemySM[] enemies;
    public bool all;
    // Use this for initialization
    public override void Start()
    {
        objtname = "Defeat " + enemies.Length + " enemies";
        
        if(all)
        {
			enemies = GameObject.FindObjectsOfType<EnemySM>();
            objtname = "Defeat all enemies!";
        }
        base.Start();

    }

    //// Update is called once per frame
    public override void Update()
    {
        int killed = 0;
        foreach(EnemySM enemy in enemies)
        {
            if (!enemy || enemy.GetComponent<HealthComponent>().health <= 0) killed++;
        }
        numCompleted = killed;
        base.Update();
    }

    public override void onFail()
    {
        om.OnFail(this.gameObject);
    }

    public override bool check()
    {
        return complete;
    }
}
