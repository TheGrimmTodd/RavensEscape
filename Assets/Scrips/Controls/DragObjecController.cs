using UnityEngine;
using System.Collections;

public class DragObjecController : MonoBehaviour {
	bool move;

	public void Move () {

		if (Input.GetMouseButtonDown (0))
			move = true;
		if (Input.GetMouseButtonDown (1))
			move = false;

		if (move) 
		{
			Vector3 location = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			location.z = 0;
			transform.position = location;
		}
	}
}
