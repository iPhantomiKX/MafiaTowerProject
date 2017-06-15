using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed;
	Vector2 _direction;
	bool isReady;

	// Use this for initialization
	void Awake () {
		isReady = false;
	}

	public void SetDirection(Vector2 direction)
	{
		_direction = direction;
		isReady = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isReady) 
		{
			Vector2 position = transform.position;

			position += _direction * speed * Time.deltaTime;

			transform.position = position;

			Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
			Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

			if (transform.position.x <= min.x 
				|| transform.position.y <= min.y
			    || transform.position.x >= max.x 
				|| transform.position.y >= max.y) 
			{
				Destroy (this.gameObject);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D otherCollider)
	{
		if (otherCollider.gameObject.tag != "Player") 
		{
			Debug.Log (otherCollider.gameObject.name);
			Destroy (this.gameObject);
		}
	}
}
