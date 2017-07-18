using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianPersuadeInspect : Inspect
{
    public override void inspect()
    {
        if (TraitHolderRef.CheckForTrait(GetComponent<TraitObstacle>().RequiredTrait))
            GetComponent<CivilianSM>().CurrentState = CivilianSM.CIVILIAN_STATE.PERSUADED;
    }

    public override void OnInspectStart()
    {
        GetComponent<CivilianSM>().CurrentState = CivilianSM.CIVILIAN_STATE.INTERACTING;
    }
}
