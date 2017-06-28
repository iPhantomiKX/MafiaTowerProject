using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class TraitSelectMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Click");
            }
        }

	}

    void OnMouseDown()
    {
        Debug.Log("Called");
        Application.LoadLevel("SomeLevel");
    }
}
