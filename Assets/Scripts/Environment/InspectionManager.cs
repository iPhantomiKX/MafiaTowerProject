using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectionManager : MonoBehaviour {
    public Canvas canvas;
    public Image inspectMenuPanel;
    public Button inspectMenuButton;
    Image activePanel;
	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();  //Added by Randall - A quick search to reference the main canvas.
	}
	
	// Update is called once per frame
	void Update () {
        InspectMenuButton[] buttons = FindObjectsOfType<InspectMenuButton>();
        if (FindObjectOfType<PlayerController>().inspectingObject == null && buttons.Length > 0)
        {
            Debug.Log("Player is no longer inspecting");
            foreach(InspectMenuButton button in buttons)
            {
                if(button.action != null)
                button.action.interrupt();
            }
            Destroy(activePanel.gameObject);
            activePanel = null;
        }
        
    }
    public Image CreateInspectionMenu(GameObject target) {
        Inspect[] options = target.GetComponents<Inspect>();
        Image panel = Instantiate(inspectMenuPanel, target.transform.position, Quaternion.identity);
        panel.transform.SetParent(canvas.transform);
        panel.transform.SetAsLastSibling();
        panel.rectTransform.anchoredPosition = transform.position;

        foreach(Inspect option in options)
        {
            if(option.gameObject.GetComponent<TraitObstacle>() != null)
            {
                if(!option.TraitHolderRef.CheckForTrait(option.gameObject.GetComponent<TraitObstacle>().RequiredTrait))
                {
                    Debug.Log("Require trait. Unable to interact.");
                    break;
                }
            }
            Button button = Instantiate(inspectMenuButton);
            Text optionText = button.GetComponentInChildren<Text>();
            optionText.text = option.actionName;
            button.transform.SetParent(panel.transform);
            button.GetComponent<InspectMenuButton>().action = option;
            button.onClick.AddListener(() => {
                if (!option.takesTime)
                {
                    option.inspect();
                    panel.enabled = false;
                    Destroy(panel.gameObject);
                    FindObjectOfType<PlayerController>().inspectingObject = null;
                    
                }
                else
                {
                    option.startTimer();
                }
                
            });
        }
        Button cancelButton = Instantiate(inspectMenuButton);
        cancelButton.GetComponentInChildren<Text>().text = "Cancel";
        cancelButton.transform.SetParent(panel.transform);
        cancelButton.onClick.AddListener(() => {
            panel.enabled = false;
            Destroy(panel.gameObject);
            FindObjectOfType<PlayerController>().inspectingObject = null;
        });
        activePanel = panel;
        return panel;
    }

}
