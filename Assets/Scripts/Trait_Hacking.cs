using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait_Hacking : TraitBaseClass {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

    public override bool Check(GameObject checkObject)
    {
		if (checkObject.name == ConditionObject.name)
        {
            return true;
        }
        else
            return false;
    }

    public override void DoEffect()
    {
        checkObject.GetComponent<Console>().OpenDoor();
    }
}
