using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PersistentData : MonoBehaviour {

	// ------------------------------------------------------ //
	// Access this class by using "PersistentData.m_Instance."
	// ------------------------------------------------------ //

	public static PersistentData m_Instance;

    public List<TraitBaseClass> AllTraits = new List<TraitBaseClass>();

    public List<TraitBaseClass> PlayerTraits = new List<TraitBaseClass>();
    public List<string> PlayerTraitNames = new List<string>();
    public int CurrentLevel;

	public bool InitialLoad = false;
	public bool LoadFailed = false;

	// Stuff to store in persistent data

	// Use this for initialization
	void Awake () {

		DontDestroyOnLoad(gameObject);

		if (!m_Instance)
		{
			m_Instance = this;
		}
		else if (m_Instance != this)
		{
			Destroy(gameObject);
		}
        LoadData();
    }

	// Update is called once per frame
	void Update () {

		if (!InitialLoad)
		{
			InitialLoad = true;
		}

		// Clear data
		if (Input.GetKeyUp(KeyCode.X))
		{
			// Load whatever numbers here
		}

        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            PlayerTraits = AllTraits;
        }

	}

	#if UNITY_ANDROID

	void OnApplicationQuit() {
	SaveDate();
	}

	void OnApplicationPause() {
	#if UNITY_EDITOR

	#else
	SaveDate();
	#endif
	}

	#endif

	void OnDisable()
	{
		SaveDate();
	}

	PlayerData CopyData()
	{
        PlayerTraitNames.Clear();
        foreach (TraitBaseClass aTrait in PlayerTraits)
        {
            PlayerTraitNames.Add(aTrait.DisplayName);
        }

		PlayerData returnData = new PlayerData();

        returnData.PlayerTraitNames = PlayerTraitNames;
        returnData.CurrentLevel = CurrentLevel;

		return returnData;
	}

	void LoadData(PlayerData theData)
	{
        Debug.Log("Load Called");

        this.PlayerTraitNames = theData.PlayerTraitNames;
        this.CurrentLevel = theData.CurrentLevel;

        // Load PlayerTraits
        for (int i = 0; i < AllTraits.Count; ++i)
        {
            foreach (string TraitName in PersistentData.m_Instance.PlayerTraitNames)
            {
                if (TraitName.Contains(AllTraits[i].DisplayName))
                {
                    PlayerTraits.Add(AllTraits[i]);
                }
            }
        }
	}

	public void SaveDate()
	{
        Debug.Log("Save Called");

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/PersistentData.dat");

		PlayerData data = CopyData();

		bf.Serialize(file, data);
		file.Close();
	}

	public void LoadData()
	{
        if (File.Exists(Application.persistentDataPath + "/PersistentData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PersistentData.dat", FileMode.Open);
			PlayerData data = (PlayerData)(bf.Deserialize(file));

			LoadData(data);
			file.Close();
		}
		else
		{
			LoadFailed = true;
		}
	}
}

[System.Serializable]
class PlayerData
{
    public List<string> PlayerTraitNames;
    public int CurrentLevel;
}

[System.Serializable]
public struct SerializableVector2
{
	public SerializableVector2(float x, float y)
	{
		f_x = x;
		f_y = y;
	}

	public SerializableVector2(Vector2 aVec2)
	{
		f_x = aVec2.x;
		f_y = aVec2.y;
	}

	public Vector2 GetVec2()
	{
		return new Vector2(f_x, f_y);
	}

	float f_x;
	float f_y;
}