using UnityEngine;
using System.Collections;

public class RevenControler : MonoBehaviour {

	
	public static float jumpHeight = 8f;

	private Vector2 FIRST = Vector2.up * jumpHeight;
	private Vector2 SECOND = Vector2.up * jumpHeight * 0.6f;
	private Vector2 WALL_RIGHT = new Vector2 (jumpHeight * 3f, jumpHeight * 0.2f);
	private Vector2 WALL_LEFT = new Vector2 (jumpHeight * -3f, jumpHeight * 0.2f);
	private Vector2 NONE = Vector2.zero;

	
	float speed = 5.0f;
	public Rigidbody2D _rigidBody;
	private bool grounded = false;
	private int jumps = 0;
	private int maxJumps = 2;
	private Transform groundCheck;
	private Transform rightWallCheck;
	private Transform leftWallCheck;
	private Transform _transform;

	private int foreground;


	public virtual void Awake()
	{
		groundCheck = transform.Find ("GroundCheck");
		rightWallCheck = transform.Find ("RightWallCheck");
		leftWallCheck = transform.Find ("LeftWallCheck");
		foreground = 1 << LayerMask.NameToLayer ("foreground");
		_transform = this.transform;

	}

	void Update () {

		Vector2 jumpVector = getJumpVector ();
		float moveHorizontal = Input.GetAxis ("Horizontal");

		 float y = _rigidBody.velocity.y;
		_rigidBody.velocity = new Vector2(moveHorizontal * speed, y);

		if (Input.GetKeyDown(KeyCode.Space))
		{

			print(jumpVector);
			if ( jumpVector.Equals (FIRST) || jumpVector.Equals (SECOND) )
			{
				jumps ++;
			}
			_rigidBody.velocity = Vector2.zero;
			_rigidBody.AddForce( jumpVector , ForceMode2D.Impulse);
		}

	}

	private bool isGrounded ()
	{
		return _rigidBody.velocity.y == 0 && 
			(Physics2D.Linecast (_transform.position, groundCheck.position, foreground)
			||Physics2D.Linecast (new Vector2(_transform.position.x - 0.16f,_transform.position.y),
			                      new Vector2(groundCheck.position.x - .16f,groundCheck.position.y), foreground)
			||Physics2D.Linecast (new Vector2(_transform.position.x + 0.16f,_transform.position.y), 
			                      new Vector2(groundCheck.position.x + .16f,groundCheck.position.y), foreground));
	}

	private bool isOnRightWall ()
	{
		return _rigidBody.velocity.x == 0 && 
			(Physics2D.Linecast (_transform.position, rightWallCheck.position, foreground)
			 ||Physics2D.Linecast (new Vector2(_transform.position.x - 0.16f,_transform.position.y), 
			                      new Vector2(rightWallCheck.position.x - .16f,rightWallCheck.position.y), foreground)
			 ||Physics2D.Linecast (new Vector2(_transform.position.x + 0.16f,_transform.position.y), 
			                      new Vector2(rightWallCheck.position.x + .16f,rightWallCheck.position.y), foreground));
	}

	private bool isOnLeftWall ()
	{
		return _rigidBody.velocity.x == 0 && 
			(Physics2D.Linecast (_transform.position, leftWallCheck.position, foreground)
			 ||Physics2D.Linecast (new Vector2(_transform.position.x - 0.16f,_transform.position.y), 
			                      new Vector2(leftWallCheck.position.x - .16f,leftWallCheck.position.y), foreground)
			 ||Physics2D.Linecast (new Vector2(_transform.position.x + 0.16f,_transform.position.y), 
			                      new Vector2(leftWallCheck.position.x + .16f,leftWallCheck.position.y), foreground));
	}

	private Vector2 getJumpVector ()
	{
		if (isGrounded ()) 
		{
			jumps = 0;
			return FIRST;
		} 
		else if (isOnRightWall ()) 
		{
			print("WALL_LEFT" + WALL_RIGHT);
			return WALL_LEFT;
		} 
		else if (isOnLeftWall ()) 
		{
			print("WALL_RIGHT" + WALL_LEFT);
			return WALL_RIGHT;
		} 
		else if (jumps < maxJumps) 
		{
			print("SECOND" + SECOND);
			return SECOND;
		} 
		else 
		{
			print("NONE" + NONE);
			return NONE;
		}
	}



}
