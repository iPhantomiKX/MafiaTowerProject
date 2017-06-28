using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAlarmInspect : Inspect
{
    public override void inspect()
    {
        if (TraitHolderRef.CheckForTrait(GetComponent<TraitObstacle>().RequiredTrait))
            GetComponent<LaserAlarm>().SwitchOff();
    }
}
