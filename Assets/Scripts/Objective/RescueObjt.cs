using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueObjt : Objective {

	// Use this for initialization
	void Start () {
		objtname = "Rescue a Person";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void doAction ()
	{
		Debug.Log ("RescuePeep");
		complete = true;
		this.GetComponent<SpriteRenderer> ().enabled = false;
		ObjectiveManager.GetComponent<ObjectiveManager> ().OnComplete (this.gameObject);
	}
}
