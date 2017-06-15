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
            go.transform.SetParent(transform);
            go.GetComponent<ButtonElement>().AttachedTrait = aTraitName;

            go.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddToList(GameObject toAdd)
    {
        GameObject go = Instantiate(ButtonPrefab) as GameObject;
        go.transform.SetParent(transform);
        go.GetComponent<ButtonElement>().AttachedTrait = toAdd.GetComponent<ButtonElement>().AttachedTrait;

        go.SetActive(true);
    }

    public void RemoveFromList(GameObject toRemove)
    {
        Transform[] TransformList = GetComponentsInChildren<Transform>();
        foreach (Transform aTransform in TransformList)
        {
            if (aTransform.gameObject == toRemove)
            {
                Destroy(aTransform.gameObject);
                break;
            }
        }
    }
}
