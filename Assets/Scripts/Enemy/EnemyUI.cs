using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour {

	public GameObject enemy;
	public Slider healthBar;
	public Animator anim;

	bool inital = true;
	float showTime;
	bool show;
	float lastHP;

	// Use this for initialization
	void Start () {
		showTime = 0f;
		show = false;
	}

	// Update is called once per frame
	void Update () {
		this.transform.position = enemy.transform.position;
		if (inital) {
			lastHP = enemy.GetComponent<HealthComponent> ().health;
			healthBar.maxValue = lastHP;
			inital = false;
		}
        float curHp = enemy.GetComponent<HealthComponent>().health;
		if (curHp != lastHP) {
			healthBar.value = curHp;
			lastHP = curHp;
			showTime = 7f;
			if (!show) {
				ShowBar ();
				show = true;
			}
		} else {
			showTime -= Time.deltaTime;
			if (showTime <= 0) {
				showTime = 0;
				if (show) {
					HideBar ();
					show = false;
				}
			}
		}
	}

	public void ShowBar(){
		show = true;
		healthBar.gameObject.SetActive (true);
		anim.Play ("EnemyHealthBarAnimIn");
	}
	 
	public void HideBar(){
		show = false;
		anim.Play ("EnemyHealthBarAnimOut");
		Invoke ("HideInvoke", 0.3f);
	}

	private void HideInvoke(){
		if(!show)
			healthBar.gameObject.SetActive (false);
	}
}
