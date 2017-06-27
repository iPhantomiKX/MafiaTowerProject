using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TraitBaseClass : MonoBehaviour {

    public enum TRAIT_TYPE
    {
        PASSIVE,    // Traits that add to player's stats
        ACTIVE,     // Traits that have a specific object to trigger
        ABILITY,    // Traits that can be used anywhere
    }

    [Header("Base Trait Values")]
    public GameObject ConditionObject;
    public TRAIT_TYPE traitType;
    public double CooldownTime = 0.0;
    public string DisplayName;
    public string DisplayDescription;

    protected GameObject checkObject;
    protected bool inputReceived;
    protected KeyCode inputKeyReceived;
    protected PlayerController playerObject;
    protected double CooldownTimer = 0.0;

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

    public void SetInput(bool status, KeyCode key)
    {
        inputReceived = status;
        inputKeyReceived = key;
    }

    public void SetPlayer(GameObject Player)
    {
        playerObject = Player.GetComponent<PlayerController>();
    }

    public void DoTrait()
    {
        if (CooldownTimer <= 0.0)
            DoEffect();

        //if (traitType == TRAIT_TYPE.PASSIVE)
        //{
        //    DoEffect();
        //}
        //else
        //{
        //    if (ConditionObject)
        //    {
        //        if (checkObject != null)
        //        {
        //            // Traits that have a specific object to interact with
        //            if (Check(checkObject))
        //            {
        //                if (inputReceived && inputKeyReceived == InputKey)
        //                {
        //                    DoEffect();
        //                    inputReceived = false;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // Traits that can be used anywhere
        //        if (CooldownTimer <= 0.0)
        //        {
        //            if (inputReceived && inputKeyReceived == InputKey)
        //            {
        //                DoEffect();
        //                inputReceived = false;
        //                CooldownTimer = CooldownTime;
        //            }
        //        }
        //        else
        //        {
        //            CooldownTimer -= Time.deltaTime;
        //        }
        //    }
        //}
    }

    public bool Check(GameObject checkObject)
    {
        if (ConditionObject == checkObject)
            return true;
        else
            return false;
    }

    public abstract void DoEffect();

    public bool GetIfAbility()
    {
        if (traitType == TRAIT_TYPE.ABILITY)
            return true;
        else
            return false;
    }

    public void DoCooldown()
    {
        CooldownTimer -= Time.deltaTime;
    }

    public double GetCooldown()
    {
        return CooldownTimer;
    }
}
