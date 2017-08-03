using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitch : TraitObstacle {

    FogManager fogManagerRef;
    bool fogOn = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
    void Update()
    {
        if (!fogManagerRef)
        {
            fogManagerRef = FindObjectOfType<FogManager>();
        }
        else
        {
            if (!fogOn)
                fogManagerRef.SwitchOff();
            else
                fogManagerRef.SwitchOn();
        }
    }

    public void Toggle()
    {
        fogOn = !fogOn;
    }

    public void Destroy()
    {
        fogManagerRef.SwitchOn();
    }

    public bool GetFogOn()
    {
        return fogOn;
    }
}
