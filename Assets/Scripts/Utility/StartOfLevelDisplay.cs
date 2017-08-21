using UnityEngine;
using UnityEngine.UI;

public class StartOfLevelDisplay : MonoBehaviour {

    public Text LevelText;

    [Space]
    public bool isBossLevel;
    public BossData bossdata;
    public Text BossSpecialText;
    public Text BossStrategyText;
    public GameObject BossTraitList;
    public GameObject BossInfoPanel;

    [Space]
    public GameObject MandatoryObjectives;
    public GameObject OptionalObjectives;
    public GameObject ObjectivesPanel;

    [Space]
    public GameObject LevelSummaryTextBlock;

    //Some references 
    private ObjectiveManager om;
    private PanelManager pm;
    private Animator animator;

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    void generateObjectiveText()
    {
        //Set panel to be active
        ObjectivesPanel.SetActive(true);

        //Populate the panel with the objectives
        om = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveManager>();
        foreach (Objective objt in om.objectives)
        {
            GameObject go;

            if (objt.mandatory)
                go = Instantiate(LevelSummaryTextBlock, MandatoryObjectives.transform);
            else
                go = Instantiate(LevelSummaryTextBlock, OptionalObjectives.transform);

            go.GetComponent<Text>().text = objt.objtText.text;
            go.GetComponent<Text>().color = objt.objtText.color;

            if (objt.isTimed)
                go.GetComponent<Text>().text += " in " + ((int)objt.time).ToString() + " seconds.";
        }
    }

    public void generateBossInfo()
    {
        Debug.Log("generateBossInfo()");
        //Search for boss GameObject
        bossdata = GameObject.Find("BossObject").GetComponent<BossData>();

        //Set panel to be active
        BossInfoPanel.SetActive(true);

        //Display Boss Special
        BossSpecialText.text = bossdata.special.m_name;

        //Display Boss Strategy
        BossStrategyText.text = bossdata.strategy.m_name;

        //Display Boss Traits
        foreach (BossTrait bt in bossdata.modifierList)
        {
            GameObject go = Instantiate(LevelSummaryTextBlock, BossTraitList.transform);
            go.GetComponent<Text>().text = bt.m_name;
        }
    }

    //For button - is basically a cleanup
    public void StartLevel()
    {
        //Resume the game 
        Time.timeScale = 1.0f;

        //Camera stop
        Camera.main.GetComponent<PlayerCamera>().enabled = true;

        //Freeze player
        GameObject.Find("PlayerObject").GetComponent<PlayerController>().freeze = false; 

        //Clear Lists
        clearLists();

        //Set the sub-panels to false
        ObjectivesPanel.SetActive(false);
        BossInfoPanel.SetActive(false);

        //Close this panel
        gameObject.SetActive(false);

        //Open the Approriate Gameplay Panels
        GameObject.Find("Canvas").GetComponent<PanelManager>().ActivatePanels(new string[] { "PlayerUI", "ObjectiveUI", "Minimap"});
    }

    //this is where we reuse the UI
    public void GenerateLevelSummary()
    {
        //Pause the game 
        Time.timeScale = 0f;

        //Camera resume
        Camera.main.GetComponent<PlayerCamera>().enabled = false;

        //Freeze player
        GameObject.Find("PlayerObject").GetComponent<PlayerController>().freeze = true; 

        //Deactivate other panels
        GameObject.Find("Canvas").GetComponent<PanelManager>().DeactivateAllPanels();
        gameObject.SetActive(true);

        //Check with Level manager if it's boss level 
        LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        isBossLevel = lm.BossLevel;

        //Display Level
        LevelText.text = "Level: " + PersistentData.m_Instance.CurrentLevel.ToString();

        //Generate approriate level info
        if (isBossLevel)
            generateBossInfo();
        else
            generateObjectiveText();

        //Play animation!
        GetComponent<Animator>().Play("LevelSummary");
    }

    public void clearLists()
    {
        foreach (Transform child in MandatoryObjectives.transform)
            Destroy(child.gameObject);

        foreach (Transform child in OptionalObjectives.transform)
            Destroy(child.gameObject);

        foreach (Transform child in BossTraitList.transform)
            Destroy(child.gameObject);
    }
}
