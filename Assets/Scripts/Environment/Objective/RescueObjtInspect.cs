using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueObjtInspect : Inspect {
    public override void inspect()
    {
        GetComponent<RescueSM>().SetState(RescueSM.RESCUE_STATE.FOLLOWING);
    }
}
