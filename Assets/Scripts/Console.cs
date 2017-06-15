using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour {

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
        Debug.Log("console enter");

        col.transform.GetComponent<TraitHolder>().SetCheckObjects(this.gameObject);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("console leave");

        col.transform.GetComponent<TraitHolder>().SetCheckObjects(null);
    }
}
