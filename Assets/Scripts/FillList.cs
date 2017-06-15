using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillList : MonoBehaviour {

    public GameObject ButtonPrefab;

	// Use this for initialization
	void Start () {
        foreach (string aTraitName in PersistentData.m_Instance.PlayerTraitNames)
        {
            GameObject go = Instantiate(ButtonPrefab) as GameObject;
            go.transform.parent = transform;
            go.GetComponent<ButtonElement>().AttachedTrait = aTraitName;

            go.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
