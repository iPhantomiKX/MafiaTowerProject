using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryObjt : Objective {
    public Item suitcase;
    public override bool check()
    {
        Inventory playerInv = FindObjectOfType<PlayerController>().GetComponent<Inventory>();
        return playerInv.ItemIsInInventory(suitcase) && !failed;
    }

    public override void onFail()
    {
        failed = true;
        om.OnFail(gameObject);
    }

    // Use this for initialization
    public override void Start () {
        objtname = "Deliver a suitcase to the exit";
        foreach (GameItem item in FindObjectsOfType<GameItem>())
        {
            item.LoadItemData();
            if (item.item.ID == 4) {
                suitcase = item.item;
                break;
            }
        }
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}
}
