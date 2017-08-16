using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait_HealthGain : PassiveTrait {

    [Header("Health Gain Trait Values")]
    public int health_amount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void DoEffect()
    {
        playerObject.GetComponentInChildren<HealthComponent>().AddHealthMod((int)(health_amount * GetLevelMultiplier()));
    }
}
