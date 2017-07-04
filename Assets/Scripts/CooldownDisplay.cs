using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CooldownDisplay : MonoBehaviour {

    public GameObject AttachedButton;

    public Image AttachedImage;
    public Text AttachedText;

	// Use this for initialization
	void Start () {
        AttachedImage.gameObject.SetActive(false);
        AttachedText.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (AttachedButton.GetComponent<SkillSlot>().AttachedTrait == null)
            return;

        if (AttachedButton.GetComponent<SkillSlot>().AttachedTrait.GetCooldown() > 0.0)
        {
            if (!AttachedImage.gameObject.activeInHierarchy)
                AttachedImage.gameObject.SetActive(true);

            if (!AttachedText.gameObject.activeInHierarchy)
                AttachedText.gameObject.SetActive(true);

            AttachedText.text = ((int)(AttachedButton.GetComponent<SkillSlot>().AttachedTrait.GetCooldown())).ToString();
        }
        else
        {
            if (AttachedImage.gameObject.activeInHierarchy)
                AttachedImage.gameObject.SetActive(false);

            if (AttachedText.gameObject.activeInHierarchy)
                AttachedText.gameObject.SetActive(false);
        }


    }
}
