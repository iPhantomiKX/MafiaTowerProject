using UnityEngine;
using System.Collections;

public class ConeVision : MonoBehaviour
{
	public int segments;
	private float xradius;
	private float yradius;
	LineRenderer line;
	BaseSM  sm;

	void Start ()
	{
		line = gameObject.GetComponent<LineRenderer>();
		sm = gameObject.GetComponent<BaseSM> ();
		line.SetVertexCount (segments+1);
		line.useWorldSpace = false;
		CreatePoints ();
	}


	void CreatePoints ()
	{
		xradius = sm.visionRange/2.7f;
		yradius = sm.visionRange/2.7f;
		line.widthMultiplier = sm.visionRange;


		float x;
		float y;
		float z = 0f;

		float angle = 0 - (sm.angleFOV);

		for (int i = 0; i < (segments+1); i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
			y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

			line.SetPosition (i,new Vector3(x,y,z) );

			angle += ((sm.angleFOV*2) / segments);
		}
	}
}