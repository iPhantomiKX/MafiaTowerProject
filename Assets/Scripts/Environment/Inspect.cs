using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inspect : MonoBehaviour, Inspectable {
    
    public TraitHolder TraitHolderRef;

    public abstract void inspect();
    public virtual void outline(bool on)
    {
        GetComponent<SpriteOutline>().color = Color.yellow;

        if (on)
            GetComponent<SpriteOutline>().enabled = true;
        else
            GetComponent<SpriteOutline>().enabled = false;
    }
}
