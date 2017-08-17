using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour {

	public GameObject PlayerRef{ get; set;}
	public Objective[] objectives;
	public GameObject canvas;
	public List<Text> objtTexts = new List<Text>();
    public float timeBarWidth;
    public float timeBarHeight;

    void Awake()
    {
    }

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas").GetComponent<PanelManager>().GetPanel("ObjectiveUI"); //Added by Randall - To reference the UIObject

        objectives = FindObjectsOfType<Objective>();
        Invoke ("GenerateObjtUI" , 0.05f);
        Invoke("LevelSummary", 0.1f);
    }

    // Update is called once per frame
    void Update () {
		
	}

	public bool IsComplete(){
		foreach (Objective objt in objectives) {
			if (objt.mandatory && !objt.check())
				return false;
		}
		return true;
	}

	public bool PickAble(Vector2 pos){
		return Vector2.Distance (PlayerRef.transform.position, pos) <= 5f;
	}

	public void GenerateObjtUI(){
		GameObject panel =  canvas.transform.GetChild (0).GetChild (1).GetChild (0).gameObject;
        //GameObject panel = canvas.transform.GetChild(0).gameObject;
		Font ArialFont = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
		for(int i = 0;i<=objectives.Length-1;i++) {
			GameObject newText = new GameObject ("Objective " + i);
			newText.transform.SetParent (panel.transform);
			newText.transform.localPosition = -Vector3.up * i * 55f;
			Text text = newText.AddComponent<Text>();
			text.font = ArialFont;
			text.material = ArialFont.material;
			text.color = Color.yellow;
            Objective thisObjt = objectives[i];
            if (!thisObjt.mandatory) text.color = Color.cyan;
            text.text = thisObjt.objtname;
            thisObjt.objtText = text;
			objtTexts.Add (text);
            if(thisObjt.isTimed)
            {
                GameObject objtTimeBar = new GameObject("Objective " + i + " Bar");
                objtTimeBar.transform.SetParent(newText.transform);
                objtTimeBar.transform.localPosition = new Vector2(0,50);
                Image timeBar = objtTimeBar.AddComponent<Image>();
                //Rect bar = new Rect(text.transform.position.x, text.transform.position.y, timeBarWidth, timeBarHeight);
                Color barColor = Color.yellow;
                if (!thisObjt.mandatory) barColor = Color.cyan;
                barColor.a = 0.4f;
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
        Text text = failObjt.GetComponent<Objective>().objtText;
        text.text = text.text + "..failed";
        text.color = Color.red;

        if (failObjt.GetComponent<Objective>().mandatory)
        {
            //FindObjectOfType<PlayerController>().GetComponent<HealthComponent>().onDeath();
            Debug.Log("Mandatory objective failed");
            failObjt.GetComponent<Objective>().enabled = false;
            SceneManager.LoadScene("LoseTraitScene");
        }

        // Added by donovan
        if (failObjt.GetComponentInChildren<Objective>().mandatory)
        {
            //FindObjectOfType<PlayerController>().GetComponent<HealthComponent>().onDeath();
            Debug.Log("Mandatory objective failed");
            failObjt.GetComponentInChildren<Objective>().enabled = false;
            SceneManager.LoadScene("LoseTraitScene");
        }

    }
    public void OnComplete(GameObject compObjt){
        foreach (Objective objt in objectives)
        {
            if(objt.gameObject == compObjt)
            {
                Text text = objt.objtText;
                text.text = text.text + "...done!";
                text.color = Color.green;
                compObjt.GetComponent<Objective>().remainingTime = 0;
                return;
            }
        }
	}

    private void LevelSummary()
    {
        GameObject ls = GameObject.Find("Canvas").GetComponent<PanelManager>().GetPanel("LevelSummary");
        ls.SetActive(true);
        ls.GetComponent<StartOfLevelDisplay>().GenerateLevelSummary();
    }

}
