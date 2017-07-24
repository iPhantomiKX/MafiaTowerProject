using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyInspect : Inspect
{
    public override void inspect()
    {
        Debug.Log("Body Inspect");
        GetComponent<BaseSM>().ToggleBodyDrag();
    }
}

