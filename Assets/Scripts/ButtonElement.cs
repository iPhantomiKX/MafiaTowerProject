using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ButtonElement : MonoBehaviour {

    public string AttachedTrait;
    public Text AttachedText;

	// Use this for initialization
	void Start () {
        AttachedText.text = AttachedTrait;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
