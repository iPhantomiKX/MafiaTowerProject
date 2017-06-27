using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillSkillbar : MonoBehaviour {

    SkillsInputManager TheInputManager;
    List<SkillSlot> SkillSlotList = new List<SkillSlot>();

	// Use this for initialization
	void Start () {

        TheInputManager = GetComponent<SkillsInputManager>();

        Transform[] TransformList = GetComponentsInChildren<Transform>();
        foreach (Transform aTransform in TransformList)
        {
            if (aTransform.gameObject.name.Contains("Button"))
                SkillSlotList.Add(aTransform.gameObject.GetComponent<SkillSlot>());
        }

        int skillIdx = 0;
        foreach (TraitBaseClass aTrait in PersistentData.m_Instance.PlayerTraits)
        {
            if (aTrait.GetIfAbility())
            {
                SkillSlotList[skillIdx].AttachedTrait = aTrait;
                ++skillIdx;
            }
            else
            {
                SkillSlotList[skillIdx].AttachedTrait = null;
            }

        }

        TheInputManager.SetSkillSlots(SkillSlotList);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
