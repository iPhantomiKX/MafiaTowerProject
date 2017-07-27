using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyInspect : Inspect
{
    public override void inspect(){ GetComponent<BaseSM>().ToggleBodyDrag(); }
}

