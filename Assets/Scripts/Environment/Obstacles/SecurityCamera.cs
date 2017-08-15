using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : NeutralSM
{
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

    GameObject playerasdf;

    public float soundInterval = 1.0f;
    private float sound_counter;

    private STATE current_state = STATE.ON;

    public override void Sense()
    {
        throw new NotImplementedException();
    }

    public override void Act(int value)
    {
        throw new NotImplementedException();
    }

    public override int Think()
    {
        throw new NotImplementedException();
    }

    public override void ProcessMessage()
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start()
    {
        current_state = STATE.ON;
        sound_counter = soundInterval;
        playerasdf = GameObject.Find("PlayerObject");

        angleFOV = detectionAngle;
        visionRange = detectionDistance;
        //GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, detectionAngle / 360.0f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        switch (current_state)
        {
            case STATE.ON:

                if (IsTargetSeen(playerasdf))
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
                if (!IsTargetSeen(playerasdf))
                {
                    sound_counter = 1.0f;
                    current_state = STATE.ON;
                }
                break;
        }
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
        if(coll.gameObject.name == "Melee" || coll.gameObject.name == "Bullet" || coll.gameObject.name == "EnemyBullet")
        {
            Destroy(coll.gameObject);
            CameraOff();
            current_state = STATE.DESTROYED;
        }
    }
}
