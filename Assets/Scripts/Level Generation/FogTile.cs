using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTile : MonoBehaviour {

    public bool isOff = false;
    public float timeToReappear;
    public float fadeSpeed;

    float timer = 0.0f;
	float alpha;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		alpha = GetComponent<SpriteRenderer> ().color.a;
        if (isOff)
        {
            timer += Time.deltaTime;

			if (alpha >= 0)
				GetComponent<SpriteRenderer> ().color -= new Color (0, 0, 0, Time.deltaTime * fadeSpeed);


            if (timer > timeToReappear)
            {
                timer = 0;
                isOff = false;
            }
        }
        else
        {
			if (alpha < 1)
				GetComponent<SpriteRenderer> ().color += new Color (0, 0, 0, Time.deltaTime * fadeSpeed);
        }
	}

    public void SwitchOff()
    {
        isOff = true;
        timer = 0;
    }
}
