using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjtInspect : Inspect {

    public override void inspect()
    {
        Objective objt = GetComponent<Objective>();
        if (!objt.complete && (objt.remainingTime > 0 || !objt.isTimed))
        {
            Debug.Log("Found an objective item!");
            objt.numCompleted++;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
