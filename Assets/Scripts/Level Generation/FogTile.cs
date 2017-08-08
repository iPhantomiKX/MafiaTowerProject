using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTile : MonoBehaviour {

    public enum FOG_LEVEL
    {
        UNEXPLORED,
        SEEN,
        BORDER,
        EXPLORED,
    }

    public enum FOG_TYPE
    {
        DARK,
        ALARM,
    }

    public bool isOff = false;
    public float timeToReappear;
    public float fadeSpeed;

    float timer = 0.0f;
    float minAlpha, maxAlpha;
    float alpha;
    FOG_LEVEL currentFogLevel = FOG_LEVEL.UNEXPLORED;
    FOG_TYPE currentFogType = FOG_TYPE.DARK;

	// Use this for initialization
	void Start () {
        SetFogLevel(FOG_LEVEL.UNEXPLORED);
	}
	
	// Update is called once per frame
	void Update () {
		
        // Update alpha
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
                SetFogLevel(FOG_LEVEL.EXPLORED);
            }
        }
        else
        {
			if (alpha < maxAlpha)
				GetComponent<SpriteRenderer> ().color += new Color (0, 0, 0, Time.deltaTime * fadeSpeed);
        }

        // Update colour based on fog type
        if (currentFogType == FOG_TYPE.DARK)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
	}

    public void SetFogLevel(FOG_LEVEL level)
    {
        timer = 0;

        currentFogLevel = level;

        switch (currentFogLevel)
        {
            case FOG_LEVEL.UNEXPLORED: 
                minAlpha = 1;
                maxAlpha = 1.0f;
                isOff = false;
                break;
            
            case FOG_LEVEL.SEEN: 
                minAlpha = 0;
                maxAlpha = 1.0f;
                isOff = true;
                break;
            
            case FOG_LEVEL.BORDER: 
                minAlpha = 0.5f;
                maxAlpha = 1.0f;
                isOff = true;
                break;

            case FOG_LEVEL.EXPLORED: 
                minAlpha = 0.5f;
                maxAlpha = 0.5f;
                isOff = false; 
                break;
        }
    }

    public FOG_LEVEL GetFogLevel()
    {
        return currentFogLevel;
    }
}
