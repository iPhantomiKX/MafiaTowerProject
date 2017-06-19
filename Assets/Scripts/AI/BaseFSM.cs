using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public abstract class FSMBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float AttackRange = 1;
    [Tooltip("Amount of delay between attacks in secs")]
    public float AttackSpeed = 1;
    public int AttackDamage = 1;
    public Vector2 GridPos;

    protected GameObject m_TargetedEnemy = null;

    protected Animator theAnimator;
    protected bool b_InGrid = false;

    double d_TImer; // DEBUG

    // Use this for initialization
    public virtual void Start()
    {
        theAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RunFSM();
    }

    public void RunFSM()
    {
        Sense();

        int actValue = Think();
        if (actValue != -1)
        {
            Act(actValue);
        }
    }

    public abstract void Sense();           // get/receive updates from the world
    public abstract int Think();            // process the updates
    public abstract void Act(int value);    // act upon any change in behaviour
 
    public GameObject GetTarget()
    {
        return m_TargetedEnemy;
    }

    protected bool GetAnimatorIsPlaying()
    {
        return (theAnimator.GetCurrentAnimatorStateInfo(0).length > theAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }
}