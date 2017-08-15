﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class HealthComponent : MonoBehaviour {

	public GameObject GO;
	public int health;
	public UnityEvent death;

	public AudioSource source;
	public AudioClip takeDamageSound;

    int origHealth;

	// Use this for initialization
	void Start () {

        origHealth = health;

        if (GetComponent<AudioSource>())
            source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		// CHEAT
		if (Input.GetKeyUp(KeyCode.K))
			onDeath ();

        // Cap health - Don
        if (health > origHealth)
            health = origHealth;
	}

	public void TakeDmg(int dmg)
	{
        if (health == 0) //Added by Randall - This ensures that onDeath is only called once
            return;

        if (takeDamageSound)
		    source.PlayOneShot (takeDamageSound , takeDamageSound.length);
		
        health -= dmg;
		if (health <= 0) 
		{
            health = 0; //Added by Randall - This ensure health will not be negative
            onDeath();
		}
	}

	public void onDeath()
	{
		death.Invoke ();
	}

    // Calulates how much health is left as a percentage
    public float CalculatePercentageHealth()
    {
        return ((float)health / (float)origHealth) * 100;
    }
}
