using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour {

	public GameObject PlayerRef{ get; set;}
	public Objective[] objectives;
	public GameObject canvas;
	public List<Text> objtTexts = new List<Text>();
    public float timeBarWidth;
    public float timeBarHeight;

	// Use this for initialization
	void Start () {
        objectives = FindObjectsOfType<Objective>();
		Invoke ("GenerateObjtUI" , 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool IsComplete(){
		foreach (Objective objt in objectives) {
			if (!objt.complete)
				return false;
		}
		return true;
	}

	public bool PickAble(Vector2 pos){
		return Vector2.Distance (PlayerRef.transform.position, pos) <= 5f;
	}

	public void GenerateObjtUI(){
		GameObject panel =  canvas.transform.GetChild (1).GetChild (1).GetChild (0).gameObject;
		Font ArialFont = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
		for(int i = 0;i<=objectives.Length-1;i++) {
			GameObject newText = new GameObject ("Objective " + i);
			newText.transform.SetParent (panel.transform);
			newText.transform.localPosition = -Vector3.up * i * 50f;
			Text text = newText.AddComponent<Text>();
			text.font = ArialFont;
			text.material = ArialFont.material;
			text.color = Color.yellow;
            Objective thisObjt = objectives[i];
			text.text = thisObjt.objtname;
			objtTexts.Add (text);
            if(thisObjt.isTimed)
            {
                GameObject objtTimeBar = new GameObject("Objective " + i + " Bar");
                objtTimeBar.transform.SetParent(newText.transform);
                objtTimeBar.transform.localPosition = new Vector2(0,30);
                Image timeBar = objtTimeBar.AddComponent<Image>();
                //Rect bar = new Rect(text.transform.position.x, text.transform.position.y, timeBarWidth, timeBarHeight);
                Color barColor = Color.yellow;
                barColor.a = 0.5f;
                timeBar.color = barColor;
                timeBar.rectTransform.pivot = new Vector2(0, 0);
                timeBar.rectTransform.anchorMin = new Vector2(0, 0.5f);
                timeBar.rectTransform.anchorMax = new Vector2(0, 0.5f);
                timeBar.rectTransform.sizeDelta = new Vector2(timeBarWidth, timeBarHeight);
                thisObjt.timeBar = timeBar.rectTransform;

                thisObjt.timeBarWidth = timeBarWidth;
                
            }
		}
		panel.SetActive (true);
		panel.GetComponent<Animator> ().Play ("ObjectiveUIAnimation");
	}

    public void OnFail(GameObject failObjt)
    {
        foreach (Text text in objtTexts)
        {
            if (text.text == failObjt.GetComponent<Objective>().objtname)
            {
                text.text = text.text + "...failed";
                text.color = Color.red;
                return;
            }

        }
    }
    public void OnComplete(GameObject compObjt){
		foreach (Text text in objtTexts) {
			if (text.text == compObjt.GetComponent<Objective> ().objtname) {
				// text.gameObject.SetActive (false);
                text.text = text.text + "...done!";
                text.color = Color.green;
                compObjt.GetComponent<Objective>().remainingTime = 0;
				return;
			}

		}
	}

}
