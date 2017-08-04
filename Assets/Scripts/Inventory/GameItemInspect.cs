using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemInspect : Inspect
{

    public override void inspect()
    {
        Inventory inv = GameObject.Find("PlayerObject").GetComponent<Inventory>();
        GameItem gameItem = GetComponent<GameItem>();

        inv.IncreaseSlotItemAmount(gameItem.item);
        bool destroy = inv.AddItem(gameItem.item);

        if (destroy)
        {
            Destroy(gameObject);
        }
        if (inv.InventoryIsFull())
        {
            inv.slotItemAmount = inv.slotAmount;
        }
    }
}
