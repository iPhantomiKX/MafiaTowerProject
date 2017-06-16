using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour {

	public GameObject ObjectiveManager;
	public bool complete{ get; set;}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){
		if (ObjectiveManager.GetComponent<ObjectiveManager>().PickAble(this.transform.position)) {
			doAction ();
		}
	}

	public abstract void doAction ();
}
