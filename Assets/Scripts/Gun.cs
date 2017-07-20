using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public GameObject GunObject;
	public GameObject bulletGO;
	public float fireRate;
	public int ammo;
	public int max;
    public float gunSoundRadius = 25;
    public float gunSoundFade = 0.02f;

	private GameObject newBulletGO;
	private float fireRateCountdown;
	private bool isShot;

	// Use this for initialization
	void Start () {
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
        if(FindObjectOfType<PlayerActionLimitObjt>() != null)
            FindObjectOfType<PlayerActionLimitObjt>().NotifyGun();

        GetComponent<EmitSound>().maxRadius = gunSoundRadius;
        GetComponent<EmitSound>().fadeSpeed = gunSoundFade;
        GetComponent<EmitSound>().emitSound();
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
