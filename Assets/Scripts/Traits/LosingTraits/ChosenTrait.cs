using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ChosenTrait : MonoBehaviour {

    public TraitBaseClass AttachedTrait;
    public Text AttachedText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetChosenTrait(GameObject toSet)
    {
        AttachedTrait = toSet.GetComponent<ButtonElement>().AttachedTrait;
        AttachedText.text = AttachedTrait.DisplayName;
    }
}
