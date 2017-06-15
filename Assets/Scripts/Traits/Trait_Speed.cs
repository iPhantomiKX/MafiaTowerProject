using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait_Speed : TraitBaseClass {

    public float speed;

    // Use this for initialization
    void Start() { 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool Check(GameObject checkObject)
    {
        if (checkObject == ConditionObject)
        {
            return true;
        }
        else
            return false;
    }

    public override void DoEffect()
    {
        //PlayerObject.modifier_speed = speed;
    }
}
