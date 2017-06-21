using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthScript : MonoBehaviour {

	public GameObject StealthRing;
	public Vector3 StealthRingScaleIncrease;
	public Vector3 StealthRingScaleDecrease;

	public Vector3 DefaultScale;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) 
		{
			StealthRing.gameObject.transform.localScale += StealthRingScaleIncrease;
			LayerMask layermask = 1 << 9;
			float radius = this.transform.localScale.x/6;
			Collider2D [] colliders = Physics2D.OverlapCircleAll (this.transform.position,radius,layermask);
			foreach (Collider2D collider in colliders) {
				if (collider.tag == "Enemy") {
					EnemyController enem = collider.gameObject.GetComponent<EnemyController> ();
					enem.SuspiciousPosition = this.transform.position;
					enem.SuspiciousTime += 2;
				}
			}
		}
		else 
		{
			StealthRing.gameObject.transform.localScale -= StealthRingScaleDecrease*Time.deltaTime;
			if (StealthRing.gameObject.transform.localScale.x <= DefaultScale.x && StealthRing.gameObject.transform.localScale.y <= DefaultScale.y) 
			{
				StealthRing.gameObject.transform.localScale = DefaultScale;
			}
		}
	}
}
	
