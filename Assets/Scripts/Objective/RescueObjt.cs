using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueObjt : Objective {

	// Use this for initialization
	public override void Start () {
		objtname = "Rescue a Person";
        base.Start();
	}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

	public override void doAction ()
	{
		Debug.Log ("RescuePeep");
		complete = true;
		this.GetComponent<SpriteRenderer> ().enabled = false;
		om.OnComplete (this.gameObject);
	}

    public override void onFail()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
        om.OnFail(this.gameObject);
    }
}
