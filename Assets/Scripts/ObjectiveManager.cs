using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour {

	public GameObject PlayerRef{ get; set;}
	public List<GameObject> objectives;
	public GameObject canvas;
	public List<Text> objtTexts = new List<Text>();


	// Use this for initialization
	void Start () {
		Invoke ("GenerateObjtUI" , 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool IsComplete(){
		foreach (GameObject objt in objectives) {
			if (!objt.GetComponent<Objective>().complete)
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
		for(int i = 0;i<=objectives.Count-1;i++) {
			GameObject newText = new GameObject ("Objective" + i);
			newText.transform.SetParent (panel.transform);
			newText.transform.localPosition = -Vector3.up * i * 35f;
			Text text = newText.AddComponent<Text>();
			text.font = ArialFont;
			text.material = ArialFont.material;
			text.color = Color.yellow;
			text.text = objectives [i].GetComponent<Objective> ().objtname;
			objtTexts.Add (text);
		}
		panel.SetActive (true);
		panel.GetComponent<Animator> ().Play ("ObjectiveUIAnimation");
	}


	public void OnComplete(GameObject compObjt){
		foreach (Text text in objtTexts) {
			if (text.text == compObjt.GetComponent<Objective> ().objtname) {
				text.gameObject.SetActive (false);
				return;
			}

		}
	}

}
