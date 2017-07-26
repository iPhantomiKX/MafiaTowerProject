using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTile : MonoBehaviour {

    public bool isOff = false;
    public float timeToReappear = 1.0f;

    float timer = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (isOff)
        {
            timer -= Time.deltaTime;
            if (timer > timeToReappear)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                timer = 0;
            }
        }
	}

    public void SwitchOff()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        isOff = true;
    }
}
