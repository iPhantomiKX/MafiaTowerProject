﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class HealthComponent : MonoBehaviour {

	public GameObject GO;
	public float health;
	public UnityEvent death;

	public AudioSource source;
	public AudioClip takeDamageSound;

    float origHealth;

    float base_health;
    float mod_health = 0;

	// Use this for initialization
	void Start () {

        base_health = health;
        origHealth = health;

        if (GetComponent<AudioSource>())
            source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		// CHEAT
		if (Input.GetKeyUp(KeyCode.K))
			onDeath ();

        // Calc health value with base and mod
        health = base_health + mod_health;

        // Cap health - Don
        if (health > origHealth + mod_health)
            health = origHealth+ mod_health;
	}

	public void TakeDmg(float dmg)
	{
        if (health == 0) //Added by Randall - This ensures that onDeath is only called once
            return;

        if (takeDamageSound)
		    source.PlayOneShot (takeDamageSound , takeDamageSound.length);

        if (base_health > 0)
            base_health -= dmg;
        else
            mod_health -= dmg;

        // Calc health value with base and mod
        health = base_health + mod_health;

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
        return ((float)health / (float)origHealth + mod_health) * 100;
    }

    public void SetHealthMod(int amount)
    {
        Debug.Log("set health mod");

        mod_health = amount;
    }

    public void AddHealth(int amount)
    {
        Debug.Log("add health mod");

        base_health += amount;
    }

    public float GetMaxHealth()
    {
        return (float)origHealth + mod_health;
    }

    public float GetHealth()
    {
        return base_health + mod_health;
    }
}
