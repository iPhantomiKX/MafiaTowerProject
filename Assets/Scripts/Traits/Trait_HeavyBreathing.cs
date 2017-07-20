using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait_HeavyBreathing : PassiveTrait
{

    [Header("HeavyBreathing Trait Values")]
    public double BreathInterval;
    public float breathSoundRadius = 10f;
    public float breathSoundFade = 0.005f;

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
            playerObject.GetComponentInChildren<EmitSound>().maxInitialRadius = breathSoundRadius * 0.1f;
            playerObject.GetComponentInChildren<EmitSound>().maxRadius = breathSoundRadius;
            playerObject.GetComponentInChildren<EmitSound>().fadeSpeed = breathSoundFade;
            playerObject.GetComponentInChildren<EmitSound>().emitSound();
            d_timer = BreathInterval * GetLevelMultiplier();
        }
        else
            d_timer -= Time.deltaTime;
    }
}
