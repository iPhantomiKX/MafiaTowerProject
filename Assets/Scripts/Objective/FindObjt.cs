using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjt : Objective {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void doAction ()
	{
		Debug.Log ("KeyPickup");
		complete = true;
		this.GetComponent<SpriteRenderer> ().enabled = false;
	}
}
