using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int slotItemAmount { get; set; }
    public int slotAmount { get; set; }
    public List<Item> items;
    public float suitcaseSpeedPenalty = 0.5f;
    private bool continueLooping;

    public class InventoryDataItem
    {
        public int ID;
        public int Amount;

        public InventoryDataItem(int id, int amount)
        {
            ID = id;
            Amount = amount;
        }
    }

    public List<InventoryDataItem> InventoryDataItems = new List<InventoryDataItem>();

    // Use this for initialization
    void Start()
    {
        slotAmount = 18;
        slotItemAmount = 0;
        items = new List<Item>();
        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
        }
    }


    public void IncreaseSlotItemAmount(Item item)
    {
        bool increaseAmount = false;
        if (InventoryDataItems.Count == 0)
        {
            increaseAmount = true;
        }
        else if (!item.Stackable)
        {
            increaseAmount = true;
        }
        else if (item.Stackable)
        {
            increaseAmount = true;
            for (int i = 0; i < InventoryDataItems.Count; i++)
            {
                if (item.ID == InventoryDataItems[i].ID)
                {
                    increaseAmount = false;
                }
            }
        }
        if (increaseAmount)
        {
            slotItemAmount++;
        }
    }

    public void ClearItemData(Item item)
    {
        for (int i = 0; i < InventoryDataItems.Count; i++)
        {
            if ((item.ID == InventoryDataItems[i].ID) && InventoryDataItems[i].Amount < 2)
            {
                InventoryDataItems.Remove(InventoryDataItems[i]);
                break;
            }
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        bool DestroyItem = true;

        // Item is stackable and is the root item
        if (itemToAdd.Stackable && !ItemIsInInventory(itemToAdd) && !InventoryIsFull())
        {
            InventoryDataItem dItem = new InventoryDataItem(itemToAdd.ID, 1);
            InventoryDataItems.Add(dItem);
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == -1)
                {
                    items[i] = itemToAdd;
                    break;
                }
            }
        }
        // Item is stackable and isn't the root item
        else if (itemToAdd.Stackable && ItemIsInInventory(itemToAdd))
        {
            for (int i = 0; i < InventoryDataItems.Count; i++)
            {
                if (itemToAdd.ID == InventoryDataItems[i].ID)
                {
                    InventoryDataItems[i].Amount++;
                    break;
                }
            }
        }
        else if (!itemToAdd.Stackable && !InventoryIsFull())
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == -1)
                {
                    items[i] = itemToAdd;
                    break;
                }
            }
        }
        else if (!itemToAdd.Stackable && InventoryIsFull())
        {
            DestroyItem = false;
        }
        if (DestroyItem) ApplyItemEffect(itemToAdd);
        return DestroyItem;
    }
    void ApplyItemEffect(Item item)
    {
        if(item.ID == 4)
        {
            Debug.Log("Apply suitcase slow");
            gameObject.GetComponent<PlayerController>().mod_speed *= suitcaseSpeedPenalty;
        } 
    }
    public void RemoveItemEffect(Item item)
    {
        if (item.ID == 4)
        {
            Debug.Log("Remove suitcase slow");
            gameObject.GetComponent<PlayerController>().mod_speed /= suitcaseSpeedPenalty;
        }

    }
    public bool InventoryIsFull()
    {
        if (slotItemAmount > slotAmount)
        {
            return true;
        }
        return false;
    }

    public bool ItemIsInInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == item.ID)
                return true;
        }
        return false;
    }

    public void RemoveItemFromInventory (Item itemToRemove)
    {
        if (ItemIsInInventory(itemToRemove))
        {
            for (int i = 0; i < InventoryDataItems.Count; i++)
            {
                if (itemToRemove.ID == InventoryDataItems[i].ID)
                {
                    InventoryDataItems[i].Amount--;
                    RemoveItemEffect(itemToRemove);
                    break;
                }
            }
        }
    }
}
