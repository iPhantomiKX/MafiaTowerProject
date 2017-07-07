using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoard : MonoBehaviour {

	List<Message> MessageList = new List<Message>();
	public List<EnemySM> EnemyList = new List<EnemySM>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (EnemyList.Count <= 0) {
			GameObject[] goList = GameObject.FindGameObjectsWithTag ("Enemy");

			foreach (GameObject go in goList) {


				if (EnemyList.Contains (go.GetComponent<EnemySM> ())) {
					continue;
				}
				EnemyList.Add (go.GetComponent<EnemySM> ());
			}
		}
		for (int i = 0; i < EnemyList.Count; i++) {
			if (EnemyList [i] == null) {
				EnemyList.RemoveAt (i);
			}
		}
	}

	public void AddMessage(Message aMessage)
	{
		//Debug.Log("Adding a message.");
		MessageList.Add (aMessage);
	}



	public Message GetMessage(int ID)
	{
		foreach (Message aMessage in MessageList)
		{
			if (aMessage.theReceiver.GetInstanceID() == ID)
			{
				Message temp = aMessage;
				MessageList.Remove(aMessage);
				//Debug.Log("Getting a message.");
				return temp;
			}
		}
		return null;
	}

	public List<EnemySM> getEnemyList(){
		return EnemyList;
	}
}