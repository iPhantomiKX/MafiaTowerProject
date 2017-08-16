using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAlarmPosition : MonoBehaviour {

    public GameObject end1;
    public GameObject end2;

    public GameObject emmitter1;
    public GameObject emmitter2;

	// Use this for initialization
	void Start () {
        emmitter1.gameObject.transform.position = end1.gameObject.transform.position;
        emmitter2.gameObject.transform.position = end2.gameObject.transform.position;
	}
}
