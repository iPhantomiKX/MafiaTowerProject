using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillList : MonoBehaviour {

    public GameObject ButtonPrefab;
    public ChosenList ChosenListRef;

	// Use this for initialization
	void Start () {
        foreach (TraitBaseClass aTrait in PersistentData.m_Instance.PlayerTraits)
        {
            GameObject go = Instantiate(ButtonPrefab) as GameObject;
            go.transform.SetParent(transform);
            go.GetComponent<ButtonElement>().AttachedTrait = aTrait;

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

    public void AddToList(TraitBaseClass toAdd)
    {
        GameObject go = Instantiate(ButtonPrefab) as GameObject;
        go.transform.SetParent(transform);
        go.GetComponent<ButtonElement>().AttachedTrait = toAdd;

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
        // Check if can transfer
        int count = 0;

        Transform[] TransformList = ChosenListRef.GetComponentsInChildren<Transform>();
        foreach (Transform aTransform in TransformList)
        {
            if (aTransform.gameObject.name.Contains("Button"))
            {
                ++count;
            }
        }

        if (count >= PersistentData.m_Instance.NumTraitsPassDown)
            return;

        // Remove from current list
        RemoveFromList(toTransfer);

        // Add to other list
        ChosenListRef.AddToList(toTransfer);
    }
}
