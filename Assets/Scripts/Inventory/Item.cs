using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Item {
    public enum ItemTypes { Weapon, Consumable}
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public ItemTypes ItemType { get; set; }
    public Sprite Sprite { get; set; }
    public int ID { get; set; }
    public string Description { get; set; }
    public string ActionName { get; set; }
    public string ItemName { get; set; }
    public string Slug { get; set; }
    // public List<BaseStat> Stats { get; s et; }
    //public bool ItemModifier { get; set; }
    // Other values to maybe add later: Stackable, Value, rarity, power
            
    [Newtonsoft.Json.JsonConstructor]
    public Item(int id, string description, ItemTypes itemType ,string actionName, string itemName, string slug)
    {
        this.ID = id;
        this.Description = description;
        this.ItemType = itemType;
        this.ActionName = actionName;
        this.ItemName = itemName;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
    }

    public Item()
    {
        this.ID = -1;
    }

}
