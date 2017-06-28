using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraArea : LaserAlarm {
    public float stayDuration;
    float stayDurationRemaining;
    public float hideDuration;
    float hideDurationRemaining;
	// Use this for initialization
	void Start () {
        StartCoroutine("Blink");
    }

    // Update is called once per frame
    void Update () {
	}
    IEnumerator Blink()
    {
        while (true)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
            yield return new WaitForSeconds(stayDuration);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(hideDuration);
        }
    }
}
