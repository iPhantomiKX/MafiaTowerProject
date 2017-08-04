using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    public int amount;
    public int slot;
    public bool instantiated;

    private InventoryDisplay invDisp;
    private Tooltip tooltip;
    private ItemMenu itemMenu;
    private Vector2 offset;


    void Start()
    {
        invDisp = GameObject.Find("Inventory_").GetComponent<InventoryDisplay>();
        tooltip = invDisp.GetComponent<Tooltip>();
        itemMenu = invDisp.GetComponent<ItemMenu>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item.ID != -1)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item.ID != -1)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(invDisp.slots[slot].transform);
        this.transform.position = invDisp.slots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerId == -2 && !itemMenu.getItemMenuExists())
        {
            itemMenu.setItemMenuExists(true);
            itemMenu.CreateItemMenu(item, slot);
        }
    }
}
