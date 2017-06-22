using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEnemy : Objective
{

    public EnemyController[] enemies;
    public bool all;
    // Use this for initialization
    public override void Start()
    {
        objtname = "Defeat enemies";
        if(all)
        {
            enemies = GameObject.FindObjectsOfType<EnemyController>();
        }
        base.Start();

    }

    //// Update is called once per frame
    public override void Update()
    {

        base.Update();
    }

    public override void doAction()
    {

    }
    public override void onFail()
    {
        this.enabled = false;
        om.OnFail(this.gameObject);
    }

}
