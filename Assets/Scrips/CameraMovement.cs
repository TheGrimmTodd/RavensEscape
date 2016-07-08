using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public const float smoothness = 0.5f;         
	private Vector2 playerVelocity;
	public GameObject target; 
	public float freeMoveZoneY= 5f;
	private bool moveY = false;

	private Vector3 velocity = Vector3.zero;
	// Use this for initialization
	void Start () {
		// Setting up the reference.
		playerVelocity = target.GetComponent<Rigidbody2D> ().velocity;
	}

	void FixedUpdate() {
		if (target) {
			followTargetX ();
			followTargetY ();
		}
	}

	private void followTargetX ()
	{
		var v = transform.position;
		v.x = target.transform.position.x;
		float velocity = playerVelocity.x == 0 ? smoothness : playerVelocity.x;
		transform.position = Vector3.Lerp (transform.position, v, System.Math.Abs (velocity) * Time.deltaTime);
	}

	private void followTargetY ()
	{

		float distance = Mathf.Abs (target.transform.position.y - transform.position.y);
		if (distance > freeMoveZoneY) 
		{
			moveY = true;
		}
		if (moveY) 
		{
			var v = transform.position;
			v.y = target.transform.position.y;
			float velocity = playerVelocity.y == 0 ? smoothness : playerVelocity.y;
			transform.position = Vector3.Lerp (transform.position, v, System.Math.Abs (velocity) * Time.deltaTime);
			if (transform.position.y == target.transform.position.y)
			{
				moveY = false;
			}
		}
	}
}
