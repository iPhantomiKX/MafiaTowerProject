using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public GameObject GunObject;
	public GameObject bulletGO;
	public float fireRate;

	private GameObject newBulletGO;
	private float fireRateCountdown;
	private bool isShot;

	// Use this for initialization
	void Start () {
		isShot = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isShot && PlayerController.shootButton) 
		{
			Shoot ();
			isShot = true;
			fireRateCountdown = fireRate;
		}
		if (isShot) 
		{
			fireRateCountdown -= 1.0f;
			if (fireRateCountdown <= 0.0f) 
			{
				fireRateCountdown = 0.0f;
				isShot = false;
				PlayerController.shootButton = false;
			}
		}
	}

	void Shoot()
	{
		newBulletGO = (GameObject)Instantiate (bulletGO);
		newBulletGO.transform.position = GunObject.transform.position;
		Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GunObject.transform.position;
		newBulletGO.GetComponent<Bullet> ().SetDirection (direction);
	}
}
