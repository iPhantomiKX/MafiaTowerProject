using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonElement : MonoBehaviour {

    public TraitBaseClass AttachedTrait;
    Text AttachedText;

	// Use this for initialization
    void Start()
    {

        AttachedText = GetComponentInChildren<Text>();

        if (AttachedTrait != null)
        {
            AttachedText.text = AttachedTrait.GetName(true);

            if (SceneManager.GetActiveScene().name.Contains("NextLevelScene"))
            {
                AttachedText.text = AttachedTrait.displayName;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
