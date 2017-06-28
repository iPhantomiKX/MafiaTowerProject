using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void glassBreak() {
        if (GetComponent<Collider2D>().enabled)
        {
            GetComponent<EmitSound>().emitSound();
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Glass broken!");
        }
        else Debug.Log("Glass already broken!");
    }

}
