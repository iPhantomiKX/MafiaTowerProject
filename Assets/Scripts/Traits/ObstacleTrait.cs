using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrait : TraitBaseClass {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void DoTrait()
    {

    }

    public virtual void DoEffect() { }

    public override TRAIT_TYPE GetTraitType()
    {
        return TRAIT_TYPE.OBSTACLE;
    }
}
