using UnityEngine;
using System.Collections;

public class RevenControler : MonoBehaviour {
	float speed = 5.0f;
	public static float jumpHeight = 0.45f;
	private bool grounded = false;
	private int jumps = 0;
	private int maxJumps = 2;
	private Transform groundCheck;
	private Transform rightWallCheck;
	private Transform leftWallCheck;
	private Transform _transform;

	private int foreground;
	// Use this for initialization
	Rigidbody2D _rigidBody;

	sealed class JumpType {
		public static readonly JumpType FIRST = new JumpType (Vector2.up * jumpHeight);
		public static readonly JumpType SECOND = new JumpType (Vector2.up * jumpHeight * 0.6f);
		public static readonly JumpType WALL_RIGHT = new JumpType (new Vector2 (jumpHeight * 0.5f, jumpHeight * 0.5f));
		public static readonly JumpType WALL_LEFT = new JumpType (new Vector2 (jumpHeight * -0.5f, jumpHeight * 0.5f));

		public readonly Vector2 jumpForce;

		private JumpType(Vector2 force)
		{
			jumpForce = force;
		}
	}

	public virtual void Awake()
	{
		groundCheck = transform.Find ("GroundCheck");
		rightWallCheck = transform.Find ("RightWallCheck");
		leftWallCheck = transform.Find ("LeftWallCheck");
		foreground = 1 << LayerMask.NameToLayer ("foreground");
		_rigidBody = GetComponent<Rigidbody2D>();
		_transform = this.transform;

	}
	// Update is called once per frame
	void Update () {
		JumpType availableJump = getAvailableJump ();

		jumps = isGrounded () ? 0 : jumps;
	
		float moveHorizontal = Input.GetAxis ("Horizontal");

		 float y = _rigidBody.velocity.y;
		_rigidBody.velocity = new Vector2(moveHorizontal * speed, y);

		if(Input.GetKeyDown(KeyCode.Space) && jumps < maxJumps){
			jumps ++;
			_rigidBody.AddForce(Vector2.up * jumpHeight,ForceMode2D.Impulse);
		}

	}

	private bool isGrounded()
	{
		return _rigidBody.velocity.y == 0 && 
			(Physics2D.Linecast (_transform.position, groundCheck.position, foreground)
			||Physics2D.Linecast (new Vector2(_transform.position.x - 0.16f,_transform.position.y),
			                      new Vector2(groundCheck.position.x - .16f,groundCheck.position.y), foreground)
			||Physics2D.Linecast (new Vector2(_transform.position.x + 0.16f,_transform.position.y), 
			                      new Vector2(groundCheck.position.x + .16f,groundCheck.position.y), foreground));
	}

	private bool isOnRightWall()
	{
		return _rigidBody.velocity.x == 0 && 
			(Physics2D.Linecast (_transform.position, rightWallCheck.position, foreground)
			 ||Physics2D.Linecast (new Vector2(_transform.position.x - 0.16f,_transform.position.y), 
			                      new Vector2(rightWallCheck.position.x - .16f,rightWallCheck.position.y), foreground)
			 ||Physics2D.Linecast (new Vector2(_transform.position.x + 0.16f,_transform.position.y), 
			                      new Vector2(rightWallCheck.position.x + .16f,rightWallCheck.position.y), foreground));
	}

	private bool isOnLeftWall()
	{
		return _rigidBody.velocity.x == 0 && 
			(Physics2D.Linecast (_transform.position, leftWallCheck.position, foreground)
			 ||Physics2D.Linecast (new Vector2(_transform.position.x - 0.16f,_transform.position.y), 
			                      new Vector2(leftWallCheck.position.x - .16f,leftWallCheck.position.y), foreground)
			 ||Physics2D.Linecast (new Vector2(_transform.position.x + 0.16f,_transform.position.y), 
			                      new Vector2(leftWallCheck.position.x + .16f,leftWallCheck.position.y), foreground));
	}

	private JumpType getAvailableJump()
	{
		return JumpType.FIRST;
	}



}
