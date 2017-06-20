using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ChosenTrait : MonoBehaviour {

    public Text AttachedText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetChosenTrait(GameObject toSet)
    {
        AttachedText.text = toSet.GetComponent<ButtonElement>().AttachedTrait;
    }
}
