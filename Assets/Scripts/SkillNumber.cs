using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SkillNumber : MonoBehaviour {

    public GameObject AttachedButton; 

	// Use this for initialization
	void Start () {

            GetComponent<Text>().text = AttachedButton.GetComponent<SkillSlot>().InputKey.ToString();

        }
	
	// Update is called once per frame
	void Update () {

	}
}
