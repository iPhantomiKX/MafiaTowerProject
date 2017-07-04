using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GetTraitCount : MonoBehaviour {

    public Text AttachedText;

	// Use this for initialization
	void Start () {

        // Calculate number of traits to pass down
        PersistentData.m_Instance.NumTraitsPassDown = PersistentData.m_Instance.CurrentLevel + 999;

        AttachedText.text = "You can pass down ( " + PersistentData.m_Instance.NumTraitsPassDown + " ) traits.";
		//AttachedText.text = "You can pass down all traits.";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
