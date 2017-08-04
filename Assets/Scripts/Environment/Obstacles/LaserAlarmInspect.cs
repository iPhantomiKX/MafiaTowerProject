using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAlarmInspect : Inspect
{
    public override void inspect()
    {
        if (GetComponent<TraitObstacle>().CheckForTrait())
            GetComponent<LaserAlarm>().SwitchOff();
    }
}
