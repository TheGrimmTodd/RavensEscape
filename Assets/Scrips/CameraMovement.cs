using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public const float smoothness = 0.7f;         
	public GameObject target; 
	public float closeEnough = 0.05f;
	public float freeMoveZoneY= 5f;

	private bool moveY = true;
	private bool moveX = true;
	private float lastTimeUpdate;
	private float deltaTime; 

	void Start () {
		lastTimeUpdate = Time.time;
		deltaTime = Time.deltaTime;
	}

	void FixedUpdate() {
		deltaTime = Time.time - lastTimeUpdate;
		lastTimeUpdate = Time.time;
		if (target) {
			followTargetX ();
			followTargetY ();
		}
	}

	private void followTargetX ()
	{
		float distance = Mathf.Abs (target.transform.position.x - transform.position.x);
		if (distance > freeMoveZoneY) 
		{
			moveX = true;
		}
		if (moveX) 
		{

			float velocity = target.GetComponent<Rigidbody2D> ().velocity.x == 0 
					? smoothness 
					: Mathf.Abs (target.GetComponent<Rigidbody2D> ().velocity.x);
			Vector3 v = new Vector3 (target.transform.position.x,
		                        transform.position.y,
		                        transform.position.z);
			transform.position = Vector3.Lerp (transform.position, v, velocity * Time.deltaTime);
			distance = Mathf.Abs (target.transform.position.x - transform.position.x);
			if (distance < closeEnough )
			{
				moveX = false;
			}
		}
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
			float velocity = target.GetComponent<Rigidbody2D> ().velocity.y == 0 
					? smoothness 
					: target.GetComponent<Rigidbody2D> ().velocity.y;
			Vector3 v = new Vector3(transform.position.x, 
			                        target.transform.position.y,
			                        transform.position.z);
			transform.position = Vector3.Lerp (transform.position, v, velocity * Time.deltaTime);

			distance = Mathf.Abs (target.transform.position.y - transform.position.y);

			if (distance < closeEnough )
			{
				moveY = false;
			}
		}
	}
}
