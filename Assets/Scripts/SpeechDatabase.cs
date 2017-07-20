using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Civilian,
    Guard,

    NUM_TYPES
}

public enum SpeechType
{
    Idle,
    Damaged,
    Alert,

    NUM_TYPES
}

//Need to make a custom inspector for this shit
[System.Serializable]
public class SpeechTextAssets
{
    public TextAsset[] speechFiles = new TextAsset[(int)SpeechType.NUM_TYPES];
}

public class SpeechDatabase : MonoBehaviour
{
    private List<List<List<string>>> SpeechData = new List<List<List<string>>>();
    public SpeechTextAssets[] SpeechTextFiles = new SpeechTextAssets[(int)UnitType.NUM_TYPES];

    // Use this for initialization
    void Awake()
    {
        PopulateData();
    }

    private void PopulateData()
    {
        for (int i = 0; i < (int)UnitType.NUM_TYPES; ++i)
        {
            List<List<string>> temp_a = new List<List<string>>();

            for (int j = 0; j < (int)SpeechType.NUM_TYPES; ++j)
            {
                List<string> temp_b = new List<string>();

                GetStringsFromTextFile(i, j, temp_b);

                temp_a.Add(new List<string>(temp_b));
            }

            SpeechData.Add(new List<List<string>>(temp_a));
        }
    }

    private void GetStringsFromTextFile(int unit_type, int speech_type, List<string> list)
    {
        string[] strings = SpeechTextFiles[unit_type].speechFiles[speech_type].text.Split('\n');
        for (int i = 0; i < strings.Length; ++i)
            list.Add(strings[i]);
    }

    public string GetRandomString(int unit_type, int speech_type)
    {
        return SpeechData
            [unit_type]
            [speech_type]
            [
                Random.Range
                (
                    0,
                    SpeechData[unit_type][speech_type].Count
                )
            ];
    }

    public string GetRandomString(UnitType unit_type, SpeechType speech_type)
    {
        return SpeechData
            [(int)unit_type]
            [(int)speech_type]
            [
                Random.Range
                (
                    0,
                    SpeechData[(int)unit_type][(int)speech_type].Count
                )
            ];
    }

    public string GetRandomString(SpeechScript comp)
    {
        return SpeechData
            [(int)comp.unitType]
            [(int)comp.speechType]
            [
                Random.Range
                (
                    0,
                    SpeechData[(int)comp.unitType][(int)comp.speechType].Count
                )
            ];
    }

}
