using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInspect : Inspect
{
    public override void inspect()
    {
        GetComponent<Door>().Open();
    }
}
