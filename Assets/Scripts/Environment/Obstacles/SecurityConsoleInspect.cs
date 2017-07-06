using UnityEngine;

public class SecurityConsoleInspect : Inspect
{
    private GameObject securityConsolePanel;

    void Awake()
    {
        securityConsolePanel = GameObject.Find("SecurityConsolePanel");
        securityConsolePanel.SetActive(false);
    }

    public override void inspect()
    {
        securityConsolePanel.SetActive(true);
        securityConsolePanel.GetComponent<CameraConsole>().OpenPanel();
    }
}
