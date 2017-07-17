using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public GameObject playerObject;
    public float lerpSpeed = 0.5f;
    public Vector3 offset = new Vector3(0,0, -1.0f);

    public bool free = false;   //defines if the camera is locked to player or free

    // Use this for initialization
    protected void Start ()
    {
		playerObject = GameObject.Find("PlayerObject");
        if (playerObject == null)
        {
            enabled = false;
            Debug.Log("Can't find Player, please make sure it is present in the current scene upon Start(). Disabling PlayerCamera script...");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!free)
            //transform.position = (playerObject.transform.position + Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f))) * 0.5f + offset;
            transform.position = Vector3.Lerp(playerObject.transform.position, (playerObject.transform.position + Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f))) * 0.5f + offset, lerpSpeed); 
    }
}
