using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GetTraitDescription : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTraitDescription(GameObject go)
    {
        GetComponent<Text>().text = go.GetComponent<ButtonElement>().AttachedTrait.displayDescription;
    }
}
