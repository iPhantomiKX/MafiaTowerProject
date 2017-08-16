using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GetTraitCount : MonoBehaviour {

    public Text AttachedText;

	// Use this for initialization
	void Start () {

        AttachedText.text = "You can pass down ( " + PersistentData.m_Instance.NumTraitsPassDown + " ) traits.";
		//AttachedText.text = "You can pass down all traits.";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
