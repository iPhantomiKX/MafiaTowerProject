using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTile : MonoBehaviour {

    public bool isOff = false;
    public float timeToReappear;
    public float fadeSpeed;

    float timer = 0.0f;
    float minAlpha, maxAlpha;
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

			if (alpha >= minAlpha)
				GetComponent<SpriteRenderer> ().color -= new Color (0, 0, 0, Time.deltaTime * fadeSpeed);


            if (timer > timeToReappear)
            {
                timer = 0;
                isOff = false;
            }
        }
        else
        {
			if (alpha < maxAlpha)
				GetComponent<SpriteRenderer> ().color += new Color (0, 0, 0, Time.deltaTime * fadeSpeed);
        }
	}

    public void SwitchOff(bool borderTile = false)
    {
        isOff = true;
        timer = 0;

        if (!borderTile)
            minAlpha = 0;
        else
            minAlpha = 0.5f;

        maxAlpha = 1;
    }
}
