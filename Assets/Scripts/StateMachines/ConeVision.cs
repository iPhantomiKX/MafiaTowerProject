using UnityEngine;
using System.Collections;

public class ConeVision : MonoBehaviour
{
	public int segments;
	private float xradius;
	private float yradius;
	LineRenderer line;
	BaseSM  sm;
    BossData bd;

	void Start ()
	{
		line = gameObject.GetComponent<LineRenderer>();
		sm = gameObject.GetComponent<BaseSM> ();
        bd = gameObject.GetComponent<BossData>();
		line.SetVertexCount (segments+1);
		line.useWorldSpace = false;
		CreatePoints ();
	}


	void CreatePoints ()
	{
        if (sm)
        {
            xradius = sm.visionRange / 2.7f;
            yradius = sm.visionRange / 2.7f;
            line.widthMultiplier = sm.visionRange;
        }
        else if (bd)
        {
            xradius = bd.m_visionDistance / 2.7f;
            yradius = bd.m_visionDistance / 2.7f;
            line.widthMultiplier = bd.m_visionDistance;
        }

		float x;
		float y;
		float z = 0f;

        float angle = 0;

        if (sm)
        {
             angle = 0 - (sm.angleFOV);
        }
        else if (bd)
        {
             angle = 0 - (bd.m_visionFOV);
        }

		for (int i = 0; i < (segments+1); i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
			y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

			line.SetPosition (i,new Vector3(x,y,z) );

            if (sm)
			    angle += ((sm.angleFOV*2) / segments);
            else if (bd)
                angle += ((bd.m_visionFOV * 2) / segments);
		}
	}
}