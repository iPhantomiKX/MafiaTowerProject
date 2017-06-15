using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class RandomTrait : MonoBehaviour {

	public List<TraitBaseClass> TraitList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RandomATrait()
	{
		int rand = Random.Range (0, TraitList.Count);

		GetComponentInChildren<Text> ().text = TraitList [rand].name;

		if (PersistentData.m_Instance.PlayerTraitNames.Count == 0) {
			PersistentData.m_Instance.PlayerTraitNames.Add (TraitList [rand].name);
		}
		else if (!PersistentData.m_Instance.PlayerTraitNames.Contains (TraitList [rand].name))
			PersistentData.m_Instance.PlayerTraitNames.Add (TraitList [rand].name);
	}
}
