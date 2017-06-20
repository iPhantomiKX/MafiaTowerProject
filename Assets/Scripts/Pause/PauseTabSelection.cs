using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PauseTabSelection : MonoBehaviour {

    public GameObject ButtonPrefab;
    public CurrentTabDisplay CurrentTabDisplayRef;

    List<Canvas> TabList = new List<Canvas>();
    bool b_ListFilled = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (CurrentTabDisplayRef.GetListLoaded() && !b_ListFilled)
        {
            TabList = CurrentTabDisplayRef.GetTabList();

            foreach (Canvas aCanvas in TabList)
            {
                GameObject go = Instantiate(ButtonPrefab) as GameObject;
                go.transform.SetParent(transform);
                go.GetComponent<TabElement>().SetCanvas(aCanvas);

                go.SetActive(true);
            }

            b_ListFilled = true;
        }
	}
}
