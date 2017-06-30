using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChosenList : MonoBehaviour {

    public GameObject ButtonPrefab;
    public FillList FillListRef;

	// Use this for initialization
	void Start () {
		
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

    public void TransferToOtherList(GameObject toTransfer)
    {
        // Remove from current list
        RemoveFromList(toTransfer);

        // Add to other list
        FillListRef.AddToList(toTransfer);
    }
}
