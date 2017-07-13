using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentEntranceScript : MonoBehaviour {

    public string ToLayer = "Vent";
    public bool HideTilesOnExit = false;
    private Collider2D ptr = null;
    private GameObject vent_tiles;

    void Awake()
    {
        //RANDALL - TODO: 
        //Change this to whatever the levelgenerator named the parent object
        vent_tiles = GameObject.Find("VentParent");
    }

    void Start()
    {
        //vent_tiles.SetActive(false);
    }

    void Update()
    {
        //RANDALL - TODO: Hide/Show the Vent Tiles using the LevelGenerator
        if (ptr != null && Input.GetKeyDown("v"))
        {
            vent_tiles.SetActive(!HideTilesOnExit);
            ptr.gameObject.layer = LayerMask.NameToLayer(ToLayer);
        }
    }
	
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.name == "PlayerObject")
            ptr = coll;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.name == "PlayerObject")
            ptr = null;
    }
}
