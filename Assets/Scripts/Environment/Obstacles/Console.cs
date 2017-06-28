using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : TraitObstacle {

    public GameObject AttachedDoor;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenDoor()
    {
        AttachedDoor.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Player"))
        {

            Debug.Log("console enter");

            col.transform.GetComponent<TraitHolder>().SetInTrigger(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Player"))
        {
            Debug.Log("console leave");

            col.transform.GetComponent<TraitHolder>().SetInTrigger(false);
        }
    }
}
