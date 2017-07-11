using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour {

    enum STATE
    {
        ON,
        OFF,
        ALERT,
        DESTROYED,
        NUM_STATES
    }

    public float detectionAngle;
    public float detectionDistance;

    public float rotationAngle;
    public float rotationSpeed;

    private GameObject player;

    public float soundInterval = 1.0f;
    private float sound_counter;

    private STATE current_state = STATE.ON;

    // Use this for initialization
    void Start()
    {
        current_state = STATE.ON;
        sound_counter = soundInterval;
        //GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, detectionAngle / 360.0f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        switch(current_state)
        {
            case STATE.ON:

                if (IsPlayerSeen())
                    current_state = STATE.ALERT;
                else
                    Rotate();

                break;

            case STATE.OFF:
            case STATE.DESTROYED:   //Can probably make some sparks particle effects
                //Do nothing LOL
                break;

            case STATE.ALERT:
                EmitSound();
                if (!IsPlayerSeen())
                    current_state = STATE.ON;
                break;
        }
    }

    protected bool IsPlayerSeen()
    {
        //Move this out -> place it into the start instead (once player spawner spawns player on AWAKE instead)
        if(!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().gameObject;

        //check player in cone and in range
        Vector3 playerDir = player.transform.position - this.transform.position;
        Vector3 forward = this.transform.up;
        float angle = Vector3.Angle(playerDir, forward);
        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (angle < detectionAngle && distance < detectionDistance)
        {

            //check if player behind any obstacle
            int layerMask = (1 << 8 | 1 << 11);
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, playerDir, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    return true;
                }
            }
            else
            {
                //In case part of the body is seen
                RaycastHit2D hit2 = Physics2D.Raycast(this.transform.position, Quaternion.AngleAxis(playerDir.z + 10f, Vector3.forward) * playerDir, Mathf.Infinity, layerMask);
                if (hit2.collider != null)
                {
                    if (hit2.collider.gameObject.tag == "Player")
                    {
                        return true;
                    }
                }
                else
                {
                    RaycastHit2D hit3 = Physics2D.Raycast(this.transform.position, Quaternion.AngleAxis(playerDir.z - 10f, Vector3.forward) * playerDir, Mathf.Infinity, layerMask);
                    if (hit3.collider != null)
                    {
                        if (hit3.collider.gameObject.tag == "Player")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        else
            return false;
    }

    void Rotate()
    {
        //IDK Why the below line does not work
        //transform.localEulerAngles.Set(0, 0, transform.localEulerAngles.z + rotationSpeed * Time.deltaTime);

        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + rotationSpeed * Time.deltaTime);

        if (transform.localEulerAngles.z > rotationAngle)
            rotationSpeed = -rotationSpeed;
    }

    public void CameraOff()
    {
        if (current_state == STATE.DESTROYED)
            return;

        current_state = STATE.OFF;
        GetComponent<LineRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void CameraOn()
    {
        if (current_state == STATE.DESTROYED)
            return;

        current_state = STATE.ON;
        GetComponent<LineRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void ToggleAlert()
    {
        if(current_state != STATE.DESTROYED)
            current_state = STATE.ALERT;

    }

    //Getters
    public bool IsDestroyed() { return current_state == STATE.DESTROYED; }
    public bool IsOn() { return current_state == STATE.ON; }
    public bool IsOff() { return current_state == STATE.OFF; }

    void EmitSound()
    {
        if(sound_counter > soundInterval)
        {
            GetComponent<EmitSound>().emitSound();
            sound_counter = 0.0f;
        }
        sound_counter += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //RANDALL - TODO
        //Check that coll is a projectile or melee attack or something - by tag or something

        CameraOff();
        current_state = STATE.DESTROYED;

        //Remember to despawn projectile
    }
}
