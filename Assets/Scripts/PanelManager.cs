using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PanelManager : MonoBehaviour {

    public Dictionary<string, GameObject> PanelList;
    public string PanelPath = @"Assets/Prefabs/UI/";
    public string[] NamesOfPanels;

    void Awake()
    {
        for (int i = 0; i < NamesOfPanels.Length; ++i)
            CreatePanel(NamesOfPanels[i]);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool CreatePanel(string panel_name)
    {
        //Not sure if path works
        GameObject new_panel = Instantiate(Resources.Load(panel_name, typeof(GameObject))) as GameObject;
        PanelList.Add(panel_name, new_panel);
        new_panel.transform.SetParent(this.gameObject.transform);

        if (new_panel == null)
            Debug.Log("aoijgaoigj");

        return true;
    }

    public GameObject GetPanel(string panel_name)
    {
        return PanelList[panel_name];
    }

}

//Build a custom inspector for this I guess
//https://unity3d.com/learn/tutorials/topics/interface-essentials/building-custom-inspector
[CustomEditor(typeof(PanelManager))]
public class LevelScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PanelManager myTarget = (PanelManager)target;

        //myTarget.PanelList = EditorGUILayout.IntField("Experience", myTarget.experience);
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
}