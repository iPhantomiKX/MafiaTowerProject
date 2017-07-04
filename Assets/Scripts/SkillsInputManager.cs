using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsInputManager : MonoBehaviour {

    List<SkillSlot> SkillsSlotList = new List<SkillSlot>();

    List<KeyCode> InputBuffer = new List<KeyCode>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            InputBuffer.Add(FetchKey());
            SetInputReceived(InputBuffer);
        }
        else
        {
            InputBuffer.Clear();
        }
    }

    public void SetSkillSlots(List<SkillSlot> aList)
    {
        SkillsSlotList = aList;
    }

    public void SetInputReceived(List<KeyCode> keyBuffer)
    {
        foreach (SkillSlot aSlot in SkillsSlotList)
        {
            if (!aSlot.AttachedTrait)
                continue;

            foreach (KeyCode key in keyBuffer)
            {
                if (key == aSlot.InputKey)
                {
                    aSlot.AttachedTrait.DoTrait();
                    InputBuffer.Clear();
                    return;
                }
            }
        }
    }

    KeyCode FetchKey()
    {
        int e = System.Enum.GetNames(typeof(KeyCode)).Length;
        for (int i = 0; i < e; i++)
        {
            if (Input.GetKey((KeyCode)i) && !InputBuffer.Contains(((KeyCode)i)))
            {
                return (KeyCode)i;
            }
        }

        return KeyCode.None;
    }

    public int GetListSize()
    {
        return SkillsSlotList.Count;
    }

    public void AssignATrait(int slotNum, AbilityTrait aTrait)
    {
        int slotIdx = slotNum - 1;

        // Check if need to add a new trait or move existing trait
        foreach (SkillSlot aSlot in SkillsSlotList)
        {
            if (aSlot.AttachedTrait == aTrait)
            {
                aSlot.AttachedTrait = null;
                break;
            }
        }

        SkillsSlotList[slotIdx].AttachedTrait = aTrait;
        foreach (SkillSlot aSlot in SkillsSlotList)
        {
            aSlot.ApplyText();
        }
    }
}
