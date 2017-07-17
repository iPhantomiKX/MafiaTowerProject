using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : PlayerCamera
{
	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        if (!free)
            transform.position = playerObject.transform.position + offset;
    }

}
