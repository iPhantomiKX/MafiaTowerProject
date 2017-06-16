using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjt : Objective {

	
	// Use this for initialization
	void Start () {
		objtname = "Find an Item";
	}
	
	// Update is called once per frame
	void Update () {	

	}

	public override void doAction ()
	{
		Debug.Log ("KeyPickup");
		complete = true;
		this.GetComponent<SpriteRenderer> ().enabled = false;
		ObjectiveManager.GetComponent<ObjectiveManager> ().OnComplete (this.gameObject);
	}
}
