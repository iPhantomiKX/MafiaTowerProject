using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInspect : Inspect
{
    public override void inspect()
    {
        Door door = GetComponent<Door>();
        if (door.closed) door.Open();
        else door.Close();
    }
}
