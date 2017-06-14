using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb;
    public float speed;

    private Vector2 velocity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Move player
        Move();
	}

    void Move()
    {
        Vector2 MoveDirectionLR = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        velocity = MoveDirectionLR * speed;
        rb.velocity = new Vector2(velocity.x, velocity.y);
    }
}
