using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float smooth = 0.5f;         
	private Vector2 pvel;
	private Transform player;           

	private Vector3 velocity = Vector3.zero;
	// Use this for initialization
	void Start () {
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag("Player").transform;
		pvel = ((Rigidbody2D)GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ()).velocity;
	}

	void FixedUpdate() {
		if (player) {
			var v = transform.position;
			v.x = player.position.x;
			float velocity = pvel.x == 0? smooth : pvel.x;
			transform.position = Vector3.Lerp(transform.position, v, System.Math.Abs(velocity) * Time.deltaTime);
		}
	}
}
