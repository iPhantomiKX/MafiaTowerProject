using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrait : TraitBaseClass
{
    [Header("Ability Trait Values")]
    public double CooldownTime = 0.0;

    double CooldownTimer;

    // Use this for initialization
    void Start()
    {
        CooldownTimer = 0.0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void DoTrait()
    {
        if (CooldownTimer <= 0.0)
        {
            DoEffect();
            CooldownTimer = CooldownTime;
        }
    }

    public virtual void DoEffect() { }

    public override TRAIT_TYPE GetTraitType()
    {
        return TRAIT_TYPE.ABILITY;
    }

    public void DoCooldown()
    {
        CooldownTimer -= Time.deltaTime;
    }

    public double GetCooldown()
    {
        return CooldownTimer;
    }
}
