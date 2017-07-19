using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianPersuadeInspect : Inspect
{
    public override void inspect()
    {
        if (TraitHolderRef.CheckForTrait(GetComponent<TraitObstacle>().RequiredTrait))
            GetComponent<CivilianSM>().StartPersuade();
    }

    public override void OnInspectStart()
    {
        GetComponent<CivilianSM>().CurrentState = CivilianSM.CIVILIAN_STATE.INTERACTING;
        base.OnInspectStart();
    }

    public override void OnInspectInterupt()
    {
        GetComponent<CivilianSM>().CurrentState = CivilianSM.CIVILIAN_STATE.IDLE;
        base.OnInspectInterupt();
    }
}
