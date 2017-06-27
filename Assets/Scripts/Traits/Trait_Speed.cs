using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait_Speed : PassiveTrait {

    [Header("Speed Trait Values")]
    public float speed;

    // Use this for initialization
    void Start() { 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void DoEffect()
    {
        playerObject.mod_speed = speed;
    }
}
