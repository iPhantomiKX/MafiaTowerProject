using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed;
	Vector2 _direction;
	bool isReady;
	Rigidbody2D bullet;

	// Use this for initialization
	void Awake () {
		bullet = GetComponent<Rigidbody2D> ();
		isReady = false;
	}

	public void SetDirection(Vector2 direction)
	{
		_direction = direction;
		_direction = _direction.normalized;
		isReady = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isReady) 
		{
			//Vector2 position = transform.position;
			//
			//Vector2 velocity = _direction * speed * Time.deltaTime;
			//
			//position += new Vector2 (velocity.x, velocity.y);
			//
			//transform.position = position;
			Vector2 velocity = _direction * speed;
			bullet.velocity = new Vector2(velocity.x, velocity.y);

			Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
			Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

			if (bullet.position.x <= min.x 
				|| bullet.position.y <= min.y
			    || bullet.position.x >= max.x 
				|| bullet.position.y >= max.y) 
			{
				Destroy (this.gameObject);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D otherCollider)
	{
		if (otherCollider.gameObject.tag != "Player") 
		{
			if (otherCollider.gameObject.tag == "Enemy") {
				otherCollider.gameObject.GetComponent<EnemySM> ().TakeDamage (1f);
			}
			Destroy (this.gameObject);
		}
	}
}
