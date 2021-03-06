﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SkillSlot : MonoBehaviour {
    
    public KeyCode InputKey;
    public AbilityTrait AttachedTrait;
    Text AttachedText;

    // Use this for initialization
    void Start()
    {
        AttachedText = GetComponentInChildren<Text>();

        if (AttachedTrait != null)
            AttachedText.text = AttachedTrait.GetName(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ApplyText()
    {
        if (AttachedTrait != null)
            AttachedText.text = AttachedTrait.GetName();
        else
            AttachedText.text = "";
    }
}
