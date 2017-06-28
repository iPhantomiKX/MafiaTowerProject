using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public bool locked;
    public int keyID;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Open()
    {
        if (PlayerHasKey() || !locked) {
            GetComponent<Collider2D>().enabled = false;
        }

    }

    bool PlayerHasKey()
    {
        //Check if the player has the key or not
        return false;
    }
}
