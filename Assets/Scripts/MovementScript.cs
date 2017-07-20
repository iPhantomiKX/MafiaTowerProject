using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Movement Script requires the GameObject to have a Rigidbody component
[RequireComponent(typeof(Rigidbody2D))]

public class MovementScript : MonoBehaviour {

    public float air_resistence;    // Not sure if should put this here or somewhere else

    private Rigidbody2D rb;

    private bool is_dashing = false;
    private Vector2 dash_dir;
    private float dash_speed;
    private float dash_distance;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
	
    void FixedUpdate()
    {
        if(is_dashing)
        { 
            rb.velocity = dash_dir * dash_speed;
            dash_distance -= Time.deltaTime * dash_speed;
            dash_speed -= Time.deltaTime * air_resistence; 

            if (dash_distance <= 0 || dash_speed <= 0)
                is_dashing = false;
        }
    }

	public void Move(Vector2 direction)
    {
        rb.velocity = direction;
    }

    public void Move(Vector2 direction, float multiplier)
    {
        rb.velocity = direction * multiplier;
    }

    public void SetToDash(Vector2 direction, float distance, float duration = 1.0f)
    {
        is_dashing = true;
        dash_distance = distance;
        dash_speed = distance / duration;
        dash_dir = direction;
    }

    Rigidbody2D GetRigidbody(){ return rb; }
}
