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
}
