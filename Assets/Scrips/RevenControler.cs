using UnityEngine;
using System.Collections;

public class RevenControler : MonoBehaviour
{

	
	public static float jumpHeight = 8f;
	private Vector2 FIRST = Vector2.up * jumpHeight;
	private Vector2 SECOND = Vector2.up * jumpHeight * 0.6f;
	private Vector2 WALL_RIGHT = new Vector2 (jumpHeight * 0.5f, jumpHeight * 0.8f);
	private Vector2 WALL_LEFT = new Vector2 (jumpHeight * -0.5f, jumpHeight * 0.8f);
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
	const float halfPlayerWidth = 0.16f;
	private int foreground;

	public virtual void Awake ()
	{
		groundCheck = transform.Find ("GroundCheck");
		rightWallCheck = transform.Find ("RightWallCheck");
		leftWallCheck = transform.Find ("LeftWallCheck");
		foreground = 1 << LayerMask.NameToLayer ("foreground");
		_transform = this.transform;

	}

	void Update ()
	{

		Vector2 jumpVector = GetJumpVector ();

		MoveHorizontal ();

		if (Input.GetKeyDown (KeyCode.Space)) {

			print (jumpVector);
			if (jumpVector.Equals (FIRST) || jumpVector.Equals (SECOND)) {
				jumps ++;
			}
			_rigidBody.velocity = Vector2.zero;
			_rigidBody.AddForce (jumpVector, ForceMode2D.Impulse);
		}

	}

	void MoveHorizontal ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float y = _rigidBody.velocity.y;
		Vector2 targetVelocity = new Vector2 (moveHorizontal * speed, y);
		_rigidBody.velocity = Vector2.Lerp (_rigidBody.velocity, targetVelocity, Time.deltaTime);
	
	}

	private bool IsGrounded ()
	{
		return _rigidBody.velocity.y == 0 && 
			(Physics2D.Linecast (_transform.position, groundCheck.position, foreground)
			|| Physics2D.Linecast (new Vector2 (_transform.position.x - halfPlayerWidth, _transform.position.y),
			                      new Vector2 (groundCheck.position.x - halfPlayerWidth, groundCheck.position.y), foreground)
			|| Physics2D.Linecast (new Vector2 (_transform.position.x + halfPlayerWidth, _transform.position.y), 
			                      new Vector2 (groundCheck.position.x + halfPlayerWidth, groundCheck.position.y), foreground));
	}

	private bool IsOnRightWall ()
	{
		return _rigidBody.velocity.x == 0 && 
			(Physics2D.Linecast (_transform.position, rightWallCheck.position, foreground)
			|| Physics2D.Linecast (new Vector2 (_transform.position.x - halfPlayerWidth, _transform.position.y), 
			                      new Vector2 (rightWallCheck.position.x - halfPlayerWidth, rightWallCheck.position.y), foreground)
			|| Physics2D.Linecast (new Vector2 (_transform.position.x + halfPlayerWidth, _transform.position.y), 
			                      new Vector2 (rightWallCheck.position.x + halfPlayerWidth, rightWallCheck.position.y), foreground));
	}

	private bool IsOnLeftWall ()
	{
		return _rigidBody.velocity.x == 0 && 
			(Physics2D.Linecast (_transform.position, leftWallCheck.position, foreground)
			|| Physics2D.Linecast (new Vector2 (_transform.position.x - halfPlayerWidth, _transform.position.y), 
			                      new Vector2 (leftWallCheck.position.x - halfPlayerWidth, leftWallCheck.position.y), foreground)
			|| Physics2D.Linecast (new Vector2 (_transform.position.x + halfPlayerWidth, _transform.position.y), 
			                      new Vector2 (leftWallCheck.position.x + halfPlayerWidth, leftWallCheck.position.y), foreground));
	}

	private Vector2 GetJumpVector ()
	{
		if (IsGrounded ()) {
			jumps = 0;
			return FIRST;
		} else if (IsOnRightWall ()) {
			return WALL_LEFT;
		} else if (IsOnLeftWall ()) {
			return WALL_RIGHT;
		} else if (jumps < maxJumps) {
			return SECOND;
		} else {
			return NONE;
		}
	}



}
