using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float smooth = 0.5f;         
	private Vector2 pvel;
	public GameObject player;           

	private Vector3 velocity = Vector3.zero;
	
	void Start () {
		pvel = player.GetComponent<Rigidbody2D> ().velocity;
	}

	void FixedUpdate() {
		if (player) {
			var v = transform.position;
			v.x = player.transform.position.x;
			float velocity = pvel.x == 0? smooth : pvel.x;
			transform.position = Vector3.Lerp(transform.position, v, System.Math.Abs(velocity) * Time.deltaTime);
		}
	}
}
