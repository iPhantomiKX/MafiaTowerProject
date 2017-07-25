using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TO DO
public interface ITraitObstacle
{
}  

public abstract class TraitObstacle : MonoBehaviour, ITraitObstacle {

    [Tooltip("Trait needed to overcome this obstacle")]
    public ObstacleTrait RequiredTrait;
    public int requiredLevel = 1;

	// Use this for initialization
	void Start () {

        if (RequiredTrait)
            requiredLevel = Mathf.Clamp(requiredLevel, 1, RequiredTrait.maxLevel);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
