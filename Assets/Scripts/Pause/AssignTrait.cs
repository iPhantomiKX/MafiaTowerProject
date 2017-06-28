using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AssignTrait : MonoBehaviour {

    public SkillsInputManager SkillsRef;
    public AbilityTrait TraitToAssign;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AssignTraitToSlot()
    {
        if (TraitToAssign)
        {
            SkillsRef.AssignATrait(GetComponent<Dropdown>().value, TraitToAssign);

            transform.parent.gameObject.SetActive(false);
            Destroy(transform.FindChild("Dropdown List").gameObject);
        }
    }
}
