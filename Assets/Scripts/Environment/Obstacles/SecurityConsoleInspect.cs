using UnityEngine;

public class SecurityConsoleInspect : Inspect
{
    public string panelName = "SecurityConsole";
    private PanelManager pm;

    void Start()
    {
        pm = GameObject.Find("Canvas").GetComponent<PanelManager>();
    }

    public override void inspect()
    {
        pm.DeactivateAllPanels();
        pm.ActivatePanel(panelName);
        pm.GetPanel(panelName).GetComponent<CameraConsole>().OpenPanel();
    }
}
