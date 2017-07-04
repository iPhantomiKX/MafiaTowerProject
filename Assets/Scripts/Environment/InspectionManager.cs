using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectionManager : MonoBehaviour {
    public Canvas canvas;
    public Image inspectMenuPanel;
    public Button inspectMenuButton;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public Image CreateInspectionMenu(GameObject target) {
        Inspect[] options = target.GetComponents<Inspect>();
        Image panel = Instantiate(inspectMenuPanel, target.transform.position, Quaternion.identity);
        panel.transform.SetParent(canvas.transform);
        panel.transform.SetAsLastSibling();
        panel.rectTransform.anchoredPosition = transform.position;

        foreach(Inspect option in options)
        {
            Button button = Instantiate(inspectMenuButton);
            Text optionText = button.GetComponentInChildren<Text>();
            optionText.text = option.actionName;
            button.transform.SetParent(panel.transform);
            button.onClick.AddListener(() => {
                option.inspect(); panel.enabled = false;
                Destroy(panel.gameObject);
                FindObjectOfType<PlayerController>().inspecting = false;
            });
        }
        Button cancelButton = Instantiate(inspectMenuButton);
        cancelButton.GetComponentInChildren<Text>().text = "Cancel";
        cancelButton.transform.SetParent(panel.transform);
        cancelButton.onClick.AddListener(() => {
            panel.enabled = false;
            Destroy(panel.gameObject);
            FindObjectOfType<PlayerController>().inspecting = false;
        });
        return panel;
    }

}
