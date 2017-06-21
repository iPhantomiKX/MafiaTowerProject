using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class RandomTrait : MonoBehaviour {

    public Button ButtonToTurnOn;

    private List<TraitBaseClass> TraitList;

	// Use this for initialization
	void Start () {
        TraitList = PersistentData.m_Instance.AllTraits;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RandomATrait()
	{
		int rand = Random.Range (0, TraitList.Count);

		GetComponentInChildren<Text> ().text = TraitList [rand].DisplayName;

		if (PersistentData.m_Instance.PlayerTraits.Count == 0) {
            PersistentData.m_Instance.PlayerTraits.Add(TraitList[rand]);
		}
        else if (!PersistentData.m_Instance.PlayerTraits.Contains(TraitList[rand]))
            PersistentData.m_Instance.PlayerTraits.Add(TraitList[rand]);

        GetComponent<Button>().interactable = false;
        ButtonToTurnOn.interactable = true;
	}
}
