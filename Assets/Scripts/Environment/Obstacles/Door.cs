using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public bool locked;
    public int keyID;
    public bool closed;
    Color color;

	// Use this for initialization
	void Start () {
        color = GetComponent<SpriteRenderer>().color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Open()
    {
        if (PlayerHasKey() || !locked) {
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<EmitSound>().emitSound();
            GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.5f);
            GetComponent<DoorInspect>().actionName = "Close";
            closed = false;
        }
        else
        {
            Debug.Log("Key not found");
        }

    }
    public void Close()
    {
        GetComponent<EmitSound>().emitSound();
        GetComponent<SpriteRenderer>().color = color;
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<DoorInspect>().actionName = "Open";
        closed = true;
    }

    bool PlayerHasKey()
    {
        //Check if the player has the key or not
        Inventory playerInv = FindObjectOfType<PlayerController>().GetComponent<Inventory>();
        Item key = ItemDatabase.Instance.FetchItemByID(keyID);
        if(playerInv.CheckIfItemIsInInventory(key))
        {
            return true;
        }
        return false;
    }

    void OnCollisionEnter2D(Collision2D coll) 
    {
        if (coll.gameObject.name.Contains("Enemy") || coll.gameObject.name.Contains("Civilian"))
        {
            //open door
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.5f);
            GetComponent<DoorInspect>().actionName = "Close";
            closed = false;
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (closed)
        {
            if (coll.gameObject.name.Contains("Enemy") || coll.gameObject.name.Contains("Civilian"))
            {
                //open door
                GetComponent<Collider2D>().isTrigger = true;
                GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.5f);
                GetComponent<DoorInspect>().actionName = "Close";
                closed = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.name.Contains("Enemy") || coll.gameObject.name.Contains("Civilian"))
        {
            //close door
            GetComponent<SpriteRenderer>().color = color;
            GetComponent<Collider2D>().isTrigger = false;
            GetComponent<DoorInspect>().actionName = "Open";
            closed = true;
        }
    }
}
