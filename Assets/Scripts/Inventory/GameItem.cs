using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour {

    public Item item { get; set; }
    private ItemDatabase database;
    private SpriteRenderer mySprite;

    // Use this for initialization
    void Start () {
        LoadItemData();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadItemData()
    {
        mySprite = GetComponent<SpriteRenderer>();
        
        // Load data from database based on ID
        //item = PersistentData.m_Instance.gameObject.GetComponent<ItemDatabase>().FetchItemByID(ID);
        //item = GameObject.Find("PersistentData").GetComponent<ItemDatabase>().FetchItemByID(ID);
        database = (ItemDatabase)FindObjectOfType(typeof(ItemDatabase));
        item = database.FetchItemBySpriteName(mySprite.sprite.name);
        // add sprite renderer plus image

    }
}
