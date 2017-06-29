using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public GameObject GunObject;
	public GameObject bulletGO;
	public float fireRate;
	public int ammo;
	public int max;

	private GameObject newBulletGO;
	private float fireRateCountdown;
	private bool isShot;

	// Use this for initialization
	void Start () {
		max = 20;
		ammo = 20;
		isShot = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isShot && PlayerController.shootButton && ammo > 0) 
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
		ammo--;
		newBulletGO = (GameObject)Instantiate (bulletGO);
		newBulletGO.transform.position = GunObject.transform.position;
		Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GunObject.transform.position;
		newBulletGO.GetComponent<Bullet> ().SetDirection (direction);
	}

	public void CollectedAmmo(int collected){
		ammo += collected;
		if (ammo > max)
			ammo = max;
	}

}
