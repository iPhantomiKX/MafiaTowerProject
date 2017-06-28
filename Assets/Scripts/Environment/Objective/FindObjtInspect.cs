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
            //GameObject pm = GameObject.FindGameObjectWithTag("PauseMenu");
            //pm.GetComponent<Inventory>().AddItem(2);
            Debug.Log("Found an objective item!");
            objt.numCompleted++;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
