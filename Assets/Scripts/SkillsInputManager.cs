using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsInputManager : MonoBehaviour {

    public TraitHolder theTraitHolder;
    List<SkillSlot> SkillsSlotList = new List<SkillSlot>(); 

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        SetInputReceived(FetchKey());
	}

    public void SetSkillSlots(List<SkillSlot> aList)
    {
        SkillsSlotList = aList;
    }

    public void SetInputReceived(KeyCode key)
    {
        //theTraitHolder.SetInputReceived(key);

        foreach (SkillSlot aSlot in SkillsSlotList)
        {
            if (!aSlot.AttachedTrait)
                continue;

            if (key == aSlot.InputKey && aSlot.AttachedTrait.GetCooldown() < 0)
                aSlot.AttachedTrait.DoTrait();
        }
    }

    KeyCode FetchKey()
    {
        int e = System.Enum.GetNames(typeof(KeyCode)).Length;
        for (int i = 0; i < e; i++)
        {
            if (Input.GetKey((KeyCode)i))
            {
                return (KeyCode)i;
            }
        }

        return KeyCode.None;
    }
}
