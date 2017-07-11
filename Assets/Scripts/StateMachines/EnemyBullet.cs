using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
	public float Damage{ get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		print (coll.gameObject.tag);
		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "VIP") {
			coll.gameObject.GetComponent<HealthComponent> ().TakeDmg ((int)Damage);
		} 


		Destroy (this.gameObject);
	}

}
