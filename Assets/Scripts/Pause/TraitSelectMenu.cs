using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TraitSelectMenu : MonoBehaviour, IPointerClickHandler
{
    public float OffsetFromClick;

    public PlayerController PlayerRef;
    public GameObject MenuRef;

    Dropdown DropdownRef;
    SkillsInputManager SkillsRef;

    // Use this for initialization
    void Start()
    {

        DropdownRef = MenuRef.GetComponentInChildren<Dropdown>();

        // SHOULD CHANGE
        PlayerRef = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>();

        SkillsRef = PlayerRef.gameObject.transform.parent.gameObject.GetComponentInChildren<SkillsInputManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && GetComponent<ButtonElement>().AttachedTrait.GetTraitType() == TraitBaseClass.TRAIT_TYPE.ABILITY)
        {
            Debug.Log("Right click");

            MenuRef.SetActive(true);

            DropdownRef.GetComponent<AssignTrait>().TraitToAssign = null;
            DropdownRef.value = 0;
            DropdownRef.ClearOptions();

            List<string> OptionsList = new List<string>();
            OptionsList.Add("");

            for (int i = 0; i < SkillsRef.GetListSize(); ++i)
            {
                OptionsList.Add("Slot " + (i + 1));
            }
            DropdownRef.AddOptions(OptionsList);
            MenuRef.transform.position = eventData.pressPosition + new Vector2(OffsetFromClick, 0);

            DropdownRef.GetComponent<AssignTrait>().SkillsRef = SkillsRef;
            DropdownRef.GetComponent<AssignTrait>().TraitToAssign = (GetComponent<ButtonElement>().AttachedTrait as AbilityTrait);
        }
    }

}