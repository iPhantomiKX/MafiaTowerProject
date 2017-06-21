using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TraitBaseClass : MonoBehaviour {

    public enum TRAIT_TYPE
    {
        PASSIVE,
        ACTIVE,
    }
    public GameObject ConditionObject;
    public bool IfRequireInput;
    public TRAIT_TYPE traitType;
    public string DisplayName;
    public string DisplayDescription;

    protected GameObject checkObject;
    protected bool inputReceived;
    protected PlayerController playerObject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

	public void StartUp()
	{
		checkObject = null;
		inputReceived = false;
		playerObject = null;
	}

    public void SetCheckObject(GameObject checkObject)
    {
        this.checkObject = checkObject;
    }

    public void SetInput(bool status)
    {
        inputReceived = status;
    }

    public void SetPlayer(GameObject Player)
    {
        playerObject = Player.GetComponent<PlayerController>();
    }

    public void DoTrait()
    {
        if (traitType == TRAIT_TYPE.PASSIVE)
        {
            DoEffect();
        }
        else
        {
            if (checkObject != null)
            {
                if (Check(checkObject))
                {
                    Debug.Log("check object correct");
                    if (inputReceived)
                    {
                        DoEffect();
                        inputReceived = false;
                    }
                }
            }
        }
    }

    public abstract bool Check(GameObject checkObject);
    public abstract void DoEffect();
}
