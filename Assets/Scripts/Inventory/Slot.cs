using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public int id;
    private Inventory inv;
    private InventoryDisplay invDisp;


    // Use this for initialization
    void Start()
    {
        inv = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
        invDisp = GameObject.Find("Inventory_").GetComponent<InventoryDisplay>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

        if (inv.items[id].ID == -1)
        {
            inv.items[droppedItem.slot] = new Item();
            inv.items[id] = droppedItem.item;
            droppedItem.slot = id;
        }
        else if (droppedItem.slot != id)
        {
            Transform item = this.transform.GetChild(0);
            item.GetComponent<ItemData>().slot = droppedItem.slot;
            item.transform.SetParent(invDisp.slots[droppedItem.slot].transform);
            item.transform.position = invDisp.slots[droppedItem.slot].transform.position;

            inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
            inv.items[id] = droppedItem.item;
            droppedItem.slot = id;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
