﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HealthComponent : MonoBehaviour {

	public GameObject GO;
	public int health;
	public UnityEvent death;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Debug
		if (Input.GetKeyUp(KeyCode.K))
			onDeath ();
	}

	public void TakeDmg(int dmg)
	{
		health -= dmg;
		if (health <= 0) 
		{
			onDeath();
		}
	}

	public void onDeath()
	{
		death.Invoke ();
	}
}
