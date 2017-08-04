using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    private GameObject phoneBG;
    private GameObject currentTab;
    private GameObject inventoryCanvas;
    private GameObject slotPanel;
    private List<Item> items;
    private Inventory inv;

    public GameObject inventorySlot;
    public GameObject inventoryItem;

    public List<GameObject> slots = new List<GameObject>();
    private bool updated = false;

    void Start()
    {
        items = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>().items;
        inv = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();

        phoneBG = GameObject.Find("PhoneBG");
        currentTab = phoneBG.transform.Find("CurrentTab").gameObject;
        inventoryCanvas = currentTab.transform.Find("Inventory_").gameObject;
        slotPanel = inventoryCanvas.transform.Find("SlotPanel").gameObject;

        for (int i = 0; i < inv.slotAmount; i++)
        {
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }
    }

    void Update()
    {
        UpdateInventory();
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            updated = false;
        }
    }

    private void UpdateInventory()
    {
        if (!updated)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if ((items[i].ID != -1) && (slots[i].transform.childCount == 0))
                {
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = items[i];
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.localPosition = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = items[i].Sprite;
                    itemObj.name = items[i].ItemName;
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Stackable && (slots[i].transform.childCount > 0))
                {
                    for (int j = 0; j < inv.InventoryDataItems.Count; j++)
                    {
                        if ((items[i].ID == inv.InventoryDataItems[j].ID) && (inv.InventoryDataItems[j].Amount > 1))
                        {
                            ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                            data.transform.GetChild(0).GetComponent<Text>().text = inv.InventoryDataItems[j].Amount.ToString();
                        }
                    }
                }
            }
        }
        updated = true;
    }
}
