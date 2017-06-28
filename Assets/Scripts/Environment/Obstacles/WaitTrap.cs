using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTrap : MonoBehaviour {
    public float waitTime;
    public float waitTimeRemaining;
    Collider2D[] walls;
    public bool activated;
	// Use this for initialization
	void Start () {
        waitTimeRemaining = waitTime;
        walls = transform.GetComponentsInChildren<Collider2D>();
        activated = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !activated)
        {
            foreach (Collider2D col in walls)
            {
                col.enabled = true;
                col.GetComponent<SpriteRenderer>().enabled = true;
                activated = true;
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            waitTimeRemaining -= Time.deltaTime;
            if(waitTimeRemaining <= 0)
            {
                foreach (Collider2D col in walls)
                {
                    col.enabled = false;
                    col.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && activated)
        {
            waitTimeRemaining = waitTime;
        }
    }
}
