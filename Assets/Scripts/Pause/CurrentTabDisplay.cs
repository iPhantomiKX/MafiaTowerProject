using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CurrentTabDisplay : MonoBehaviour {

    List<Canvas> TabList = new List<Canvas>();
    bool b_ListLoaded = false;

	// Use this for initialization
	void Start () {

        GameObject[] GoList = GameObject.FindGameObjectsWithTag("PauseMenuScreen");
        foreach (GameObject go in GoList)
        {
            TabList.Add(go.GetComponent<Canvas>());
        }

        b_ListLoaded = true;

        foreach (Canvas aCanvas in TabList)
        {
            aCanvas.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCurrentTabDisplay(GameObject aCanvasGO)
    {
        Canvas aCanvas = aCanvasGO.GetComponent<TabElement>().GetCanvas();
        if (TabList.Contains(aCanvas))
        {
            Debug.Log("Contains");

            foreach (Canvas canvas in TabList)
            {
                if (canvas == aCanvas)
                    canvas.gameObject.SetActive(true);
                else
                    canvas.gameObject.SetActive(false);
            }
        }
    }

    public bool GetListLoaded()
    {
        return b_ListLoaded;
    }

    public List<Canvas> GetTabList()
    {
        return TabList;
    }

}
