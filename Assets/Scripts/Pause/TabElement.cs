using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TabElement : MonoBehaviour {

    Canvas AttachedCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCanvas(Canvas aCanvas)
    {
        AttachedCanvas = aCanvas;
        GetComponentInChildren<Text>().text = aCanvas.name;
    }

    public Canvas GetCanvas()
    {
        return AttachedCanvas;
    }
}
