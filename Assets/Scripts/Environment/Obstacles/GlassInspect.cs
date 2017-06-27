using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassInspect : Inspect {
    public override void inspect()
    {
        GetComponent<Glass>().glassBreak();
    }

}
