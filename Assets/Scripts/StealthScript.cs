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
		}
		else 
		{
			StealthRing.gameObject.transform.localScale -= StealthRingScaleDecrease;
			if (StealthRing.gameObject.transform.localScale.x <= DefaultScale.x && StealthRing.gameObject.transform.localScale.y <= DefaultScale.y) 
			{
				StealthRing.gameObject.transform.localScale = DefaultScale;
			}
		}
	}
}
