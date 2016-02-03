using UnityEngine;
using System.Collections;

public class EndZoneBehavior : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			LevelSignalSender.Instance.signalEndZoneTouched();
		}
	}
}
