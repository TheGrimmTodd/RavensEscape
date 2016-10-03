using UnityEngine;
using System.Collections;

public class Panning : MonoBehaviour , RavenController.RavenTouchedListener {

	public float dis = 10.0f;
	public float velocity = 1.5f;
	private bool dir = true;
	private Transform _transform;
	private Vector3 origional;
	private Vector3 destination;
	private Vector2 vel;

	// Use this for initialization
	void Start () {
		_transform = this.transform;
		origional = copyConstructor (_transform.position);
		destination = new Vector3( origional.x + dis, origional.y,origional.z);

		vel = GetComponent<Rigidbody2D>().velocity;
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

	public void OnRavenTouchedLeft(RavenController raven){

	}
	public void OnRavenTouchedRight(RavenController raven){
		
	}
	public void OnRavenTouchedTop(RavenController raven){
		
	}
	public void OnRavenTouchedBottom(RavenController raven){
		raven.AddExternalVelocity (vel);
	}
	public void OnRavenLeaving(RavenController raven){
		raven.AddExternalVelocity (Vector2.zero);
	}
	
}
