using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjt : Objective {

    public int itemID;
    // Use this for initialization
    public override void Start()
    {
        objtname = "Find an Item";
        //Inspect check = GetComponent<Inspect>();
        //Debug.Log(check);
        base.Start();

    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public override void onFail()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        om.OnFail(this.gameObject);
    }

    public override bool check()
    {
        return complete;
    }
}
