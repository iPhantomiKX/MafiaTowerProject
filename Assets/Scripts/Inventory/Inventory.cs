using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private int slotAmount = 18;
    //public bool itemAddedToInventory = false;
    private ItemDatabase database;

    public List<Item> items = new List<Item>();
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
        //database = PersistentData.m_Instance.gameObject.GetComponent<ItemDatabase>();
        //database = GameObject.Find("PersistentData").GetComponent<ItemDatabase>();
        database = (ItemDatabase)FindObjectOfType(typeof(ItemDatabase));
        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
        }
    }


    public int GetSlotAmount()
    {
        return slotAmount;
    }


    public void AddItem(GameItem gameItem)
    {
        //Item itemToAdd = database.FetchItemByID(id);
        Item itemToAdd = gameItem.item;

        InventoryDataItem dataItem = new InventoryDataItem(itemToAdd.ID, 1);
        InventoryDataItems.Add(dataItem);

        if (!InventoryIsFull())
        {
            // If item is stackable and isn't the root item
            if (itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd)) 
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
            else
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
        }
        else
        {
            // TODO:
            // display message along the lines of "Inventory is full!" to the player
            Debug.Log("inventory is full!");
        }

    }

    public bool InventoryIsFull()
    {
        if (InventoryDataItems.Count >= GetSlotAmount())
            return true;

        return false;
    }

    bool CheckIfItemIsInInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == item.ID)
                return true;
        }
        return false;
    }
}
