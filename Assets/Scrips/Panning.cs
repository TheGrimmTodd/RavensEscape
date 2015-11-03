using UnityEngine;
using System.Collections;

public class Panning : MonoBehaviour {

	public float dis = 10.0f;
	public float velocity = 1.5f;
	private bool dir = true;
	private Transform _transform;
	private Vector3 origional;
	private Vector3 destination;

	// Use this for initialization
	void Start () {
		_transform = this.transform;
		origional = copyConstructor (_transform.position);
		destination = new Vector3( origional.x + dis, origional.y,origional.z);

	}
	Vector3 copyConstructor(Vector3 toCopy){
		Vector3 copy = new Vector3 ();
		copy.x = toCopy.x;
		copy.y = toCopy.y;
		copy.z = toCopy.z;
		return copy;
	}
	
	// Update is called once per frame
	void Update () {


//		_transform.position = Vector3.(_transform.position, destination, velocity * Time.deltaTime);
		Vector2 vel = GetComponent<Rigidbody2D>().velocity;
		vel.x = velocity * (!dir ? -1f : 1f);
		GetComponent<Rigidbody2D> ().velocity = vel;

		if (dir && _transform.position.x > destination.x) {
			dir = false;
			destination.x -= dis;
		}
		if (!dir && _transform.position.x < destination.x) {
			dir = true;
			destination.x += dis;
		}

	}
}
