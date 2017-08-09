using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour
{
    public Image itemMenuPanel;
    public Button itemMenuButton;
    public GameObject medkit;
    public GameObject pistol;

    private List<GameObject> itemObjects { get; set; }
    private Tooltip tooltip;
    private PlayerController player;

    private InventoryDisplay inventoryDisp;
    private Inventory inv;
    private bool itemMenuExists = false;
    private bool removeItem = false;

    // Use this for initialization
    void Start()
    {
        inventoryDisp = GetComponent<InventoryDisplay>();
        tooltip = GetComponent<Tooltip>();
        inv = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>();
        itemObjects = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool setItemMenuExists(bool value)
    {
        itemMenuExists = value;
        return itemMenuExists;
    }
    public bool getItemMenuExists()
    {
        return itemMenuExists;
    }

    public void CreateItemMenu(Item item, int slot)
    {
        Image panel = Instantiate(itemMenuPanel, Input.mousePosition, Quaternion.identity);
        panel.transform.SetParent(inventoryDisp.transform);
        panel.transform.SetAsLastSibling();

        // Action button ///////////////////////////////////
        Button actionButton = Instantiate(itemMenuButton);
        Text actionText = actionButton.GetComponentInChildren<Text>();
        if (item.ItemType == Item.ItemTypes.Consumable)
        {
            actionText.text = "Use";
        }
        else if (item.ItemType == Item.ItemTypes.Weapon)
        {
            actionText.text = "Equip";
        }
        else if (item.ItemType == Item.ItemTypes.KeyItem)
        {
            actionText.text = "Equip";
        }
        actionButton.transform.SetParent(panel.transform);
        actionButton.onClick.AddListener(() =>
        {
            // TODO:
            // Add some functionality
            DoItemAction(item);
            RemoveItem(item, slot);

            itemMenuExists = false;
            panel.enabled = false;
            Destroy(panel.gameObject);
        });

        // Discard button ///////////////////////////////////
        Button discardButton = Instantiate(itemMenuButton);
        discardButton.GetComponentInChildren<Text>().text = "Discard";
        discardButton.transform.SetParent(panel.transform);
        discardButton.onClick.AddListener(() =>
        {
            removeItem = true;
            int dataItemIndex = 0;
            for (int i = 0; i < inv.InventoryDataItems.Count; i++)
            {
                if ((inv.InventoryDataItems[i].ID == item.ID) && (inv.InventoryDataItems[i].Amount > 1))
                {
                    removeItem = false;
                    dataItemIndex = i;
                    break;
                }
            }

            if (removeItem)
            {
                inv.slotItemAmount--;
                inv.ClearItemData(item);
                Debug.Log("SLOT ITEM AMOUNT after decreasing by 1: " + inv.slotItemAmount);

                inv.items[slot] = new Item();
                Transform itemTrans = inventoryDisp.slots[slot].transform;
                Destroy(itemTrans.GetChild(0).gameObject);
            }
            else
            {
                inv.InventoryDataItems[dataItemIndex].Amount--;

                if (inv.InventoryDataItems[dataItemIndex].Amount > 1)
                {
                    ItemData data = inventoryDisp.slots[slot].transform.GetChild(0).GetComponent<ItemData>();
                    data.transform.GetChild(0).GetComponent<Text>().text = inv.InventoryDataItems[dataItemIndex].Amount.ToString();
                }
                if (inv.InventoryDataItems[dataItemIndex].Amount < 2)
                {
                    ItemData data = inventoryDisp.slots[slot].transform.GetChild(0).GetComponent<ItemData>();
                    data.transform.GetChild(0).GetComponent<Text>().text = " ";
                }
            }

            if (item.ItemName == "Medkit")
            {
                Instantiate(medkit, new Vector3(player.transform.position.x, player.transform.position.y), Quaternion.identity);
            }
            else if (item.ItemName == "Pistol")
            {
                Instantiate(pistol, new Vector3(player.transform.position.x, player.transform.position.y), Quaternion.identity);
            }

            itemMenuExists = false;
            panel.enabled = false;
            Destroy(panel.gameObject);
        });

        // Cancel button ///////////////////////////////////
        Button cancelButton = Instantiate(itemMenuButton);
        cancelButton.GetComponentInChildren<Text>().text = "Cancel";
        cancelButton.transform.SetParent(panel.transform);
        cancelButton.onClick.AddListener(() =>
        {
            itemMenuExists = false;
            panel.enabled = false;
            Destroy(panel.gameObject);
        });

    }

    void DoItemAction(Item item)
    {
        switch (item.ID)
        {
            case 0:
                {
                    GameObject.FindObjectOfType<PlayerController>().gameObject.GetComponent<HealthComponent>().health += 1;
                    break;
                }

            case 1:
                {
                    break;
                }

            case 2:
                {
                    break;
                }
        }
    }

    void RemoveItem(Item item, int slot)
    {
        removeItem = true;
        int dataItemIndex = 0;
        for (int i = 0; i < inv.InventoryDataItems.Count; i++)
        {
            if ((inv.InventoryDataItems[i].ID == item.ID) && (inv.InventoryDataItems[i].Amount > 1))
            {
                removeItem = false;
                dataItemIndex = i;
                break;
            }
        }

        if (removeItem)
        {
            inv.slotItemAmount--;
            inv.ClearItemData(item);
            Debug.Log("SLOT ITEM AMOUNT after decreasing by 1: " + inv.slotItemAmount);

            inv.items[slot] = new Item();
            Transform itemTrans = inventoryDisp.slots[slot].transform;
            Destroy(itemTrans.GetChild(0).gameObject);
        }
        else
        {
            inv.InventoryDataItems[dataItemIndex].Amount--;

            if (inv.InventoryDataItems[dataItemIndex].Amount > 1)
            {
                ItemData data = inventoryDisp.slots[slot].transform.GetChild(0).GetComponent<ItemData>();
                data.transform.GetChild(0).GetComponent<Text>().text = inv.InventoryDataItems[dataItemIndex].Amount.ToString();
            }
            if (inv.InventoryDataItems[dataItemIndex].Amount < 2)
            {
                ItemData data = inventoryDisp.slots[slot].transform.GetChild(0).GetComponent<ItemData>();
                data.transform.GetChild(0).GetComponent<Text>().text = " ";
            }
        }
    }

}
