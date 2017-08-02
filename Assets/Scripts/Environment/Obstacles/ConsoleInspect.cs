using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleInspect : Inspect
{
    public override void inspect()
    {
        if (GetComponent<TraitObstacle>().CheckForTrait())
            GetComponent<Console>().OpenDoor();
    }


}
