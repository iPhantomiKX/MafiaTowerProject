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
            closed = false;
        }

    }
    public void Close()
    {
        GetComponent<EmitSound>().emitSound();
        GetComponent<SpriteRenderer>().color = color;
        GetComponent<Collider2D>().isTrigger = false;
        closed = true;
    }

    bool PlayerHasKey()
    {
        //Check if the player has the key or not
        
        return false;
    }
}
