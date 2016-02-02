using UnityEngine;
using System.Collections;

public class RevenControler : MonoBehaviour {
	float speed = 5.0f;
	public float jumpHeight = 0.45f;
	private bool grounded = false;
	private int jumps = 0;
	private int maxJumps = 2;
	private Transform groundCheck;
	private Transform _transform;

	private int foreground;
	// Use this for initialization
	Rigidbody2D _rigidBody;

	public virtual void Awake()
	{
		groundCheck = transform.Find ("GroundCheck");
		foreground = 1 << LayerMask.NameToLayer ("foreground");
		_rigidBody = GetComponent<Rigidbody2D>();
		_transform = this.transform;

	}
	// Update is called once per frame
	void Update () {
		jumps = checkGrounded () ? 0 : jumps;
	
		float moveHorizontal = Input.GetAxis ("Horizontal");

		 float y = _rigidBody.velocity.y;
		_rigidBody.velocity = new Vector2(moveHorizontal * speed, y);

		if(Input.GetKeyDown(KeyCode.Space) && jumps < maxJumps){
			jumps ++;
			_rigidBody.AddForce(Vector2.up * jumpHeight,ForceMode2D.Impulse);
		}

	}

	private bool checkGrounded()
	{

		return _rigidBody.velocity.y == 0 && 
			(Physics2D.Linecast (_transform.position, groundCheck.position, foreground)
			||Physics2D.Linecast (new Vector2(_transform.position.x - 0.16f,_transform.position.y), new Vector2(groundCheck.position.x - .16f,groundCheck.position.y), foreground)
				||Physics2D.Linecast (new Vector2(_transform.position.x + 0.16f,_transform.position.y), new Vector2(groundCheck.position.x + .16f,groundCheck.position.y),foreground));
	}



}
