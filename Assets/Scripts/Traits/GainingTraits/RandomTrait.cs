using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class RandomTrait : MonoBehaviour {

	public List<TraitBaseClass> TraitList;
    public Button ButtonToTurnOn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RandomATrait()
	{
		int rand = Random.Range (0, TraitList.Count);

		GetComponentInChildren<Text> ().text = TraitList [rand].DisplayName;

		if (PersistentData.m_Instance.PlayerTraitNames.Count == 0) {
            PersistentData.m_Instance.PlayerTraitNames.Add(TraitList[rand].DisplayName);
		}
        else if (!PersistentData.m_Instance.PlayerTraitNames.Contains(TraitList[rand].DisplayName))
            PersistentData.m_Instance.PlayerTraitNames.Add(TraitList[rand].DisplayName);

        GetComponent<Button>().interactable = false;
        ButtonToTurnOn.interactable = true;
	}
}
