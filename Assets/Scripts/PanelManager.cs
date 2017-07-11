using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Manager class that collectively manages the different Panels developers have made.
/// 
/// The PanelManager uses a single Canvas to display UI objects.
/// 
/// UI objects are Instantiated on Awake to reduce overhead from instantiating UI objects during runtime. 
/// 
/// Developers are to attach their UI Object prefab in the "PanelsToInit" dynamic array.
/// And then decide if the UI Object should active upon the start of the game.
/// 
/// Known bugs and stuff:
/// -Stretching images to fit onto the canvas doesn't really work. Will look into that.
/// 
/// </summary>

[System.Serializable]
public class UIObject
{
    public string name;
    public bool activeOnStart;
    public GameObject UIPrefab;
}

public class PanelManager : MonoBehaviour {

    public UIObject[] UIObjectList;

    private Dictionary<string, GameObject> PanelList = new Dictionary<string, GameObject>();

    //Creates the Panel list and instantiates all the specified UI Objects.
    void Awake()
    {
        for (int i = 0; i < UIObjectList.Length; ++i)
            CreatePanel(UIObjectList[i].name, UIObjectList[i].UIPrefab, UIObjectList[i].activeOnStart);
    }

    //Creates a panel based on a prefab and adds it to the PanelList Manager. (Uses the prefab's gameobject.name as the key).
    public bool CreatePanel(GameObject panel_prefab, bool active_on_init = false)
    {
        if (PanelList.ContainsKey(panel_prefab.name))
        {
            Debug.Log("Panel of that name already exists");
            return false;
        }

        GameObject new_panel = Instantiate(panel_prefab, this.gameObject.transform);
        PanelList.Add(panel_prefab.name, new_panel);

        new_panel.GetComponent<RectTransform>().localPosition = Vector2.zero;
        new_panel.SetActive(active_on_init);

        Debug.Log(panel_prefab.name + " added to panel list");

        return true;
    }

    //Creates a panel based on a prefab and adds it to the PanelList Manager. (Uses the supplied string as the key).
    public bool CreatePanel(string panel_name, GameObject panel_prefab, bool active_on_init = false)
    {
        if (PanelList.ContainsKey(panel_name))
        {
            Debug.Log("Panel of that name already exists");
            return false;
        }

        GameObject new_panel = Instantiate(panel_prefab, this.gameObject.transform);
        PanelList.Add(panel_name, new_panel);

        new_panel.GetComponent<RectTransform>().localPosition = Vector2.zero;
        new_panel.SetActive(active_on_init);

        Debug.Log(panel_name + " added to panel list");

        return true;
    }

    //Returns the UIObject from the PanelList map.
    public GameObject GetPanel(string panel_name)
    {
        if (PanelList.ContainsKey(panel_name))
            return PanelList[panel_name];
        else
        {
            Debug.Log("No such panel exists");
            return null;
        }
    }

    //Deactivates the specified UI object
    public bool ActivatePanel(string panel_name)
    {
        if(!PanelList.ContainsKey(panel_name))
        {
            Debug.Log("No such panel exists");
            return false;
        }

        PanelList[panel_name].SetActive(true);

        return true;
    }

    public void ActivatePanels(string[] panel_names)
    {
        foreach (string name in panel_names)
            ActivatePanel(name);
    }

    //Activates the specified UI object
    public bool DeactivatePanel(string panel_name)
    {
        if (!PanelList.ContainsKey(panel_name))
        {
            Debug.Log("No such panel exists");
            return false;
        }

        PanelList[panel_name].SetActive(false);

        return true;
    }

    public void DeactivatePanels(string[] panel_names)
    {
        foreach (string name in panel_names)
            DeactivatePanel(name);
    }

    public void DeactivateAllPanels()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    void OnApplicationQuit()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
}