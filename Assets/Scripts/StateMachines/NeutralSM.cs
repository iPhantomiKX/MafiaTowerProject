using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NeutralSM : BaseSM {

    [Tooltip("Player's spawn point")]
    public GameObject ExitPoint;

	// Use this for initialization
    public virtual void Start()
    {
        ExitPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");
	}

    protected override bool IsTargetSeen(GameObject target)
    {
        //check player in cone and in range
        Vector3 targetDir = target.transform.position - this.transform.position;
        Vector3 forward = this.transform.up;
        float angle = Vector2.Angle(targetDir, forward);
        float distance = Vector2.Distance(target.transform.position, this.transform.position);
        if (angle < angleFOV && distance < visionRange)
        {
            //check if player behind any obstacle
            //int layerMask = (1 << 8 | 1 << 11 | 1 << 12);

            int layerMask = Physics2D.DefaultRaycastLayers;

            if (target.GetComponent<BaseSM>())
                layerMask = LayerMask.GetMask("Player", "Default", "Interactables");
            else
                layerMask = LayerMask.GetMask("Interactables");

            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, targetDir, Mathf.Infinity);
            if (hit.collider != null)
            {
                if (CheckValidTarget(hit.collider.gameObject))
                {
                    return true;
                }
            }
            else
            {
                //In case part of the body is seen
                RaycastHit2D hit2 = Physics2D.Raycast(this.transform.position, Quaternion.AngleAxis(targetDir.z + 10f, Vector3.forward) * targetDir, Mathf.Infinity);
                if (hit2.collider != null)
                {
                    if (CheckValidTarget(hit2.collider.gameObject))
                    {
                        return true;
                    }
                }
                else
                {
                    RaycastHit2D hit3 = Physics2D.Raycast(this.transform.position, Quaternion.AngleAxis(targetDir.z - 10f, Vector3.forward) * targetDir, Mathf.Infinity);
                    if (hit3.collider != null)
                    {
                        if (CheckValidTarget(hit3.collider.gameObject))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        return false;
    }

    // Check if the checkObject is relevant to the SM
    protected override bool CheckValidTarget(GameObject checkObject)
    {
        if (checkObject == this.gameObject)
            return false;

        // Checking for a State Machine
        if (checkObject.GetComponent<BaseSM>())
        {
            // Checking for dead StateMachine
            return checkObject.GetComponent<BaseSM>().IsDead();
        }

        return false;
    }
}
