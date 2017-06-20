using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Item {
    public enum ItemTypes { Weapon, Consumable}
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public ItemTypes ItemType { get; set; }
    // public List<BaseStat> Stats { get; s et; }
    public string ObjectSlug { get; set; }
    public string Description { get; set; }
    public string ActionName { get; set; }
    public string ItemName { get; set; }
    //public bool ItemModifier { get; set; }

    [Newtonsoft.Json.JsonConstructor]
    public Item(string objectSlug, string description, ItemTypes itemType ,string actionName, string itemName)
    {
        this.ObjectSlug = objectSlug;
        this.Description = description;
        this.ItemType = itemType;
        this.ActionName = actionName;
        this.ItemName = itemName;
    }

}
