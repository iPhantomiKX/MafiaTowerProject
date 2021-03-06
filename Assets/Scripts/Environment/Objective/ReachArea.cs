﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachArea : Objective
{

    public float stayTimer;
    public float stayTimerRemain;
    // Use this for initialization
    public override void Start()
    {
        objtname = "Secure a designated area";
        stayTimerRemain = stayTimer;
        base.Start();

    }

    //// Update is called once per frame
    public override void Update()
    {
        if(stayTimerRemain <= 0 && !complete)
        {
            complete = true;
            GetComponent<SpriteRenderer>().enabled = false;
            om.OnComplete(this.gameObject);
        }
        base.Update();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            stayTimerRemain -= Time.deltaTime;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !complete)
        {
            stayTimerRemain = stayTimer;
        }
    }
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
