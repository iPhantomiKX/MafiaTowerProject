using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour {

	public GameObject PlayerRef{ get; set;}
	public List<GameObject> objectives;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool IsComplete(){
		foreach (GameObject objt in objectives) {
			if (!objt.GetComponent<Objective>().complete)
				return false;
		}
		return true;
	}

	public bool PickAble(Vector2 pos){
		return Vector2.Distance (PlayerRef.transform.position, pos) <= 2f;
	}


}
