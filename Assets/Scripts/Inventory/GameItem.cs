using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    public Item item { get; set; }
    private ItemDatabase database;
    private SpriteRenderer mySprite;

    // Use this for initialization
    void Start()
    {
        LoadItemData();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadItemData()
    {
		mySprite = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
        database = (ItemDatabase)FindObjectOfType(typeof(ItemDatabase));
        item = database.FetchItemBySpriteName(mySprite.sprite.name);
    }
}
