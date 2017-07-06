using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public GameObject playerObject;
    public Vector3 offset = new Vector3(0,0, -1.0f);

    public bool free = false;   //defines if the camera is locked to player or free

    // Use this for initialization
    void Start ()
    {
		playerObject = GameObject.Find("PlayerObject"); 
    }
	
	// Update is called once per frame
	void Update ()
    {
        //TODO: Once player spawner is changed to spawn player on awake, can remove this
        if (!playerObject)
            playerObject = GameObject.Find("PlayerObject");

        if (!free)
            transform.position = (playerObject.transform.position + Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f))) * 0.5f + offset;
    }
}
