using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjt : Objective {


    // Use this for initialization
    public override void Start()
    {
        objtname = "Find an Item";
        base.Start();

    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public override void doAction ()
	{
		Debug.Log ("KeyPickup");
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
