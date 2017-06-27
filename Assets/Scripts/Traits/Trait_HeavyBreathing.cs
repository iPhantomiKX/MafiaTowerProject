using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait_HeavyBreathing : PassiveTrait
{

    [Header("HeavyBreathing Trait Values")]
    public double BreathInterval;

    double d_timer = 0.0;

    // Use this for initialization
    void Start()
    {
        d_timer = BreathInterval;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void DoEffect()
    {
        if (d_timer <= 0.0)
        {
            playerObject.GetComponentInChildren<StealthScript>().ExpandRing();
            d_timer = BreathInterval;
        }
        else
            d_timer -= Time.deltaTime;
    }
}
