using System.Collections;
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
	}

	public void TakeDmg(int dmg)
	{
		source.PlayOneShot (takeDamageSound , takeDamageSound.length);
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

    // Calulates how much health is left as a percentage
    public float CalculatePercentageHealth()
    {
        return (health / origHealth) * 100;
    }
}
