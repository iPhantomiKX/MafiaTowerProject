using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectMenuButton : MonoBehaviour {
    public Inspect action;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (action != null && action.takesTime)
        {
            RectTransform timeBar = transform.GetChild(0).GetComponent<RectTransform>();
            Vector2 barSize = timeBar.sizeDelta;

            barSize.x = (action.remainingTime / action.time) * GetComponent<RectTransform>().sizeDelta.x;
            timeBar.sizeDelta = barSize;
        }
    }
}
