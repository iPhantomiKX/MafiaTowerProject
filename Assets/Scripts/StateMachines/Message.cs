using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message  {

	public enum MESSAGE_TYPE

	{
		ENEMY_SPOTPLAYER,   // Enemy Spot player and warn others
		ENEMY_AREACLEAR		// Send Out that the area is clear
	}

	public MESSAGE_TYPE theMessageType;
	public GameObject theSender, theReceiver, theTarget;
	public Vector3 theDestination;
}