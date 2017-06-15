﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb;
    public float speed;

	public static bool shootButton;

    private Vector2 velocity;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Move player
        Move();
		FaceMousePos();
		Shootbutton();
	}

    void Move()
    {
        Vector2 MoveDirectionLR = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        velocity = MoveDirectionLR * speed;
        rb.velocity = new Vector2(velocity.x, velocity.y);
    }

	void FaceMousePos()
	{
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = Input.mousePosition - pos;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	bool Shootbutton()
	{
		if (Input.GetMouseButtonDown (0)) {
			shootButton = true;
		} 
		else
			shootButton = false;

		return shootButton;
	}
}
