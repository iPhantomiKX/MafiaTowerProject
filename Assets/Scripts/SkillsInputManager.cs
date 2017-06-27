using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsInputManager : MonoBehaviour {

    List<SkillSlot> SkillsSlotList = new List<SkillSlot>(); 

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
            SetInputReceived(FetchKey());
        else
            SetInputReceived(KeyCode.None);
    }

    public void SetSkillSlots(List<SkillSlot> aList)
    {
        SkillsSlotList = aList;
    }

    public void SetInputReceived(KeyCode key)
    {
        foreach (SkillSlot aSlot in SkillsSlotList)
        {
            if (!aSlot.AttachedTrait)
                continue;

            if (key == aSlot.InputKey)
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
