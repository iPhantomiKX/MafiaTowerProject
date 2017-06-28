﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    private Item item;
    private string data;
    private GameObject tooltip;

    void Start()
    {
        tooltip = GameObject.Find("Tooltip");
        tooltip.SetActive(false);
    }

    void Update()
    {
        if(tooltip.activeSelf)
        {
            tooltip.transform.position = Input.mousePosition;
        }
    }


    public void Activate(Item item)
    {
        this.item = item;
        ConstructDataString();
        tooltip.SetActive(true);
    }

    public void Deactivate()
    {
        tooltip.SetActive(false);
    }

    public void ConstructDataString()
    {
        //0473f0
        //data = "<color=#0473f0><b>" + item.ItemName + "</b></color>\n\n" + item.Description + "";
        data = "whatthefuckduckdsdfoijsdofijsodfijsodifjsdofj\nisdofjisodisdfsdfsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsd\nsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdj";
        //data = "<color=#0473f0><b>" + item.ItemName + "";
        tooltip.transform.GetChild(0).GetComponent<Text>().text = data; 
    }

}
