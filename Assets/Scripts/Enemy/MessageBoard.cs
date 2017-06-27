using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoard : MonoBehaviour {

	List<Message> MessageList = new List<Message>();


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
}