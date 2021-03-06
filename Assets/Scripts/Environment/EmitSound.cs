﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TeamHandler))]
public class EmitSound : MonoBehaviour {
    public float maxInitialRadius = 20f;
    public float expandSpeed = 0.75f;
    public float stayExpandSpeed = 0.1f;
    public float maxRadius = 25f;
    public float fadeSpeed = 0.02f;
    public SoundCircleController sc;
    // Use this for initialization
    void Start () {
	    	
	}
    public void emitSound()
    {
        sc.maxInitialRadius = maxInitialRadius;
        sc.expandSpeed = expandSpeed;
        sc.stayExpandSpeed = stayExpandSpeed;
        sc.maxRadius = maxRadius;
        sc.fadeSpeed = fadeSpeed;

        if (GetComponent<TeamHandler>())
            sc.senderTeam = GetComponent<TeamHandler>().currentTeam;
        else if (GetComponentInParent<TeamHandler>())
            sc.senderTeam = GetComponentInParent<TeamHandler>().currentTeam;
        else
            sc.senderTeam = TeamHandler.TEAM.NEUTRAL;

        Instantiate(sc, transform.position, Quaternion.identity);
    }
	
}
