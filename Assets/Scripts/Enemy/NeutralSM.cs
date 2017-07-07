using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NeutralSM : BaseSM {

    [Tooltip("Player's spawn point")]
    public GameObject ExitPoint;

	// Use this for initialization
    public virtual void Start()
    {
		
	}
}
