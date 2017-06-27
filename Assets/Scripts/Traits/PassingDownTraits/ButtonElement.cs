using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ButtonElement : MonoBehaviour {

    public TraitBaseClass AttachedTrait;
    Text AttachedText;

	// Use this for initialization
	void Start () {

        AttachedText = GetComponentInChildren<Text>();

        if (AttachedTrait != null)
            AttachedText.text = AttachedTrait.DisplayName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
