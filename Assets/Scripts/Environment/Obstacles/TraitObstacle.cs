﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TO DO
//public interface ITraitObstacle
//{
//}  

public class TraitObstacle : MonoBehaviour {

    [Tooltip("Trait needed to overcome this obstacle")]
    public ObstacleTrait RequiredTrait;
    public int requiredLevel = 1;

    public TraitHolder traitHolderRef;

	// Use this for initialization
	void Start () {

        if (RequiredTrait)
            requiredLevel = Mathf.Clamp(requiredLevel, 1, RequiredTrait.maxLevel);

	}
	
	// Update is called once per frame
	public void Update () {

        if (!traitHolderRef)
        {
            traitHolderRef = FindObjectOfType<TraitHolder>();    
        }  
	}

    public bool CheckForTrait()
    {
        return (traitHolderRef.CheckForTrait(RequiredTrait, requiredLevel));
    }
}
