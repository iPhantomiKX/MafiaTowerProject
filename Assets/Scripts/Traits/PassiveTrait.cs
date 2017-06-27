using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveTrait : TraitBaseClass {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void DoTrait()
    {
        DoEffect();
    }

    public virtual void DoEffect() { }

    public override TRAIT_TYPE GetTraitType()
    {
        return TRAIT_TYPE.PASSIVE;
    }

}
