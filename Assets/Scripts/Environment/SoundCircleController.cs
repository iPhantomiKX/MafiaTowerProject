﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCircleController : MonoBehaviour {
    public float maxInitialRadius = 20f;
    public float expandSpeed = 0.75f;
    public float stayExpandSpeed = 0.1f;
    public float maxRadius = 25f;
    public float fadeSpeed = 0.02f;

    public TeamHandler.TEAM senderTeam;
	
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, fadeSpeed);
        if(transform.localScale.x >= maxRadius)
        {
            Destroy(this.gameObject);
        }
        else if(transform.localScale.x >= maxInitialRadius)
        {
            transform.localScale += (Vector3)new Vector2(stayExpandSpeed, stayExpandSpeed);
        }
     
        else
        {
            transform.localScale += (Vector3)new Vector2(expandSpeed, expandSpeed);
        }
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.GetComponent<TeamHandler>().CheckIfCanInteract(senderTeam))
            {
                if (other.GetComponent<EnemySM>())
                    other.GetComponent<EnemySM>().StartSuspicious(this.transform.position, 5f);
            }
        }

        if (other.GetComponent<CivilianSM>())
        {
            if (other.GetComponent<TeamHandler>().CheckIfCanInteract(senderTeam))
                other.GetComponent<CivilianSM>().StartRunning();
        }
    }   
}
