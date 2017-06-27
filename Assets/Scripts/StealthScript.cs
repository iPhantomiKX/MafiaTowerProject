using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthScript : MonoBehaviour {

	public GameObject StealthRing;
	public Vector3 StealthRingScaleIncrease;
	public Vector3 StealthRingScaleDecrease;

	public Vector3 DefaultScale;

    bool b_RingExpanding = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
            b_RingExpanding = true;

		if (b_RingExpanding) 
		{
            ExpandRing();
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

    public void ExpandRing()
    {
        StealthRing.gameObject.transform.localScale += StealthRingScaleIncrease;
        LayerMask layermask = 1 << 9;
        float radius = this.transform.localScale.x / 6;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, radius, layermask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
				EnemySM enem = collider.gameObject.GetComponent<EnemySM>();
                enem.SuspiciousPosition = this.transform.position;
                enem.SuspiciousTime = 10f;
            }
        }

        b_RingExpanding = false;
    }
}
	
