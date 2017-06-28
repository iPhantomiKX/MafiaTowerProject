using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; set; }
    private List<Item> Items { get; set; }

    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        BuildDatabase();
    }

    private void BuildDatabase()
    {
        Items = JsonConvert.DeserializeObject<List<Item>>(Resources.Load<TextAsset>("Json/Items").ToString());
        //Debug.Log(Items[0].ActionName);
    }

    public Item FetchItemByID(int id)
    {
        foreach (Item item in Items)
        {
            if (item.ID == id)
                return item;
        }
        Debug.LogWarning("couldn't find item with id: " + id);
        return null;
    }
}
