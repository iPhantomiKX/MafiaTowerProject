using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemInspect : Inspect {

    public override void inspect()
    {
        GameObject inv = GameObject.Find("PlayerObject");
        GameItem item = GetComponent<GameItem>();

        if(!inv.GetComponent<Inventory>().InventoryIsFull())
        {
            inv.GetComponent<Inventory>().AddItem(item);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
