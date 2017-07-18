using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inspect : MonoBehaviour, Inspectable {
    
    public TraitHolder TraitHolderRef;
    public string actionName;
    public bool takesTime;
    public float time, remainingTime;
    bool inspecting;
    public abstract void inspect();
    public virtual void outline(bool on)
    {
        GetComponent<SpriteOutline>().color = Color.yellow;

        if (on)
            GetComponent<SpriteOutline>().enabled = true;
        else
            GetComponent<SpriteOutline>().enabled = false;
    }
    public void Start()
    {
        remainingTime = time;
        inspecting = false;
    }
    public void Update()
    {
        if (takesTime && inspecting) {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else
            {
                this.inspect();
                interrupt();
                FindObjectOfType<PlayerController>().inspectingObject = null;
            }
        }
        
    }
    public void interrupt()
    {
        if(takesTime && inspecting)
        {
            Debug.Log("Action interrupted!");
            remainingTime = time;
            inspecting = false;
        }
    }
    public void startTimer()
    {
        inspecting = true;
    }

    // Function to handle anything that should be done when inspecting timer starts
    public virtual void OnInspectStart()
    {

    }

}
