using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inspect : MonoBehaviour, Inspectable {
    
    public TraitHolder TraitHolderRef;

    public abstract void inspect();
}
