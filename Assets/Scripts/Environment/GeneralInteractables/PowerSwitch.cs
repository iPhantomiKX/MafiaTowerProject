using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitch : TraitObstacle {

    FogManager fogManagerRef;
    bool fogOn = false;
    bool changeStatus = false;

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
        else if (fogManagerRef && changeStatus)
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
        changeStatus = true;

    }

    public void Destroy()
    {
        fogOn = true;
        fogManagerRef.SwitchOn();

        Destroy(this.gameObject);
    }

    public bool GetFogOn()
    {
        return fogOn;
    }
}
