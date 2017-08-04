using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitchInspect : Inspect {

    public override void inspect()
    {
        if (GetComponent<TraitObstacle>().CheckForTrait())
        {
            GetComponent<PowerSwitch>().Toggle();

            if (GetComponent<PowerSwitch>().GetFogOn())
                actionName = "Power Off";
            else
                actionName = "Power On";
        }
    }
}
