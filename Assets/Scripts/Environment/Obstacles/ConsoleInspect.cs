using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleInspect : Inspect
{
    public override void inspect()
    {
        if (TraitHolderRef.CheckForTrait(GetComponent<TraitObstacle>().RequiredTrait, GetComponent<TraitObstacle>().requiredLevel))
            GetComponent<Console>().OpenDoor();
    }


}
