using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RavenController : MonoBehaviour
{

	
	public static float jumpHeight = 6.5f;
	private Vector2 FIRST = Vector2.up * jumpHeight;
	private Vector2 SECOND = Vector2.up * jumpHeight * 0.6f;
	private Vector2 WALL_RIGHT = new Vector2 (jumpHeight * 0.5f, jumpHeight * 0.8f);
	private Vector2 WALL_LEFT = new Vector2 (jumpHeight * -0.5f, jumpHeight * 0.8f);
	private Vector2 NONE = Vector2.zero;
	float speed = 7.0f;
	public Rigidbody2D _rigidBody;
	private bool grounded = false;
	private int jumps = 0;
	private int maxJumps = 2;
	private Transform groundCheck;
	private Transform rightWallCheck;
	private Transform leftWallCheck;
	private Transform _transform;
	private Vector2 externalVelocity;

	private int foreground;
	private RavenTouchedListener groundListener;
	private RavenTouchedListener topListener;
	private RavenTouchedListener rightListener;
	private RavenTouchedListener leftListener;
	public BoxCollider2D collider;
	private Vector2 lowerLeft;
	private Vector2 lowerRight;
	private Vector2 upperLeft;
	private Vector2 upperRight;

	private Vector2 startLine;
	private Vector2 endLine;

	public float colliderSkin;

	private float halfPlayerWidth;
	private float halfPlayerHeight;


	public virtual void Awake ()
	{
		groundCheck = transform.Find ("GroundCheck");
		rightWallCheck = transform.Find ("RightWallCheck");
		leftWallCheck = transform.Find ("LeftWallCheck");
		foreground = 1 << LayerMask.NameToLayer ("foreground");
		_transform = this.transform;


	}

	void Start(){
		lowerLeft = new Vector2 ();
		lowerRight = new Vector2 ();
		upperLeft = new Vector2 ();
		upperRight = new Vector2 ();
		startLine = new Vector2 ();
		endLine = new Vector2 ();
		externalVelocity = new Vector2 (0, 0);

		halfPlayerWidth = collider.bounds.size.x/2;
		halfPlayerHeight = collider.bounds.size.y / 2;
	}

	void FixedUpdate ()
	{
		findCorners ();

		Vector2 jumpVector = GetJumpVector ();

		MoveHorizontal ();

		if (Input.GetKeyDown (KeyCode.Space)) {

			print (jumpVector);
			if (jumpVector.Equals (FIRST) || jumpVector.Equals (SECOND)) {
				jumps ++;
			}

			if(jumpVector != NONE) _rigidBody.velocity = Vector2.zero;

			_rigidBody.AddForce (jumpVector, ForceMode2D.Impulse);

		}

	}

	void MoveHorizontal ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float y = _rigidBody.velocity.y;
		Vector2 targetVelocity = new Vector2 (moveHorizontal * speed, y);
		targetVelocity =  targetVelocity + externalVelocity;
		_rigidBody.velocity = Vector2.Lerp (_rigidBody.velocity, targetVelocity, Time.deltaTime);
		_rigidBody.velocity = targetVelocity;

	}

	void findCorners ()
	{
		lowerLeft.x = collider.bounds.center.x - halfPlayerWidth - colliderSkin;
		lowerLeft.y = collider.bounds.center.y - halfPlayerHeight - colliderSkin;
		lowerRight.x = collider.bounds.center.x + halfPlayerWidth + colliderSkin;
		lowerRight.y = collider.bounds.center.y - halfPlayerHeight - colliderSkin;
		upperLeft.x = collider.bounds.center.x - halfPlayerWidth - colliderSkin;
		upperLeft.y = collider.bounds.center.y + halfPlayerHeight + colliderSkin;
		upperRight.x = collider.bounds.center.x + halfPlayerWidth + colliderSkin;
		upperRight.y = collider.bounds.center.y + halfPlayerHeight + colliderSkin;
	}

	private bool IsGrounded ()
	{
		startLine.x = lowerLeft.x + colliderSkin;
		startLine.y = lowerLeft.y;
		endLine.x = lowerRight.x - colliderSkin;
		endLine.y = lowerRight.y;

		Debug.DrawLine (startLine, endLine);

		RaycastHit2D hit = Physics2D.Linecast (startLine, endLine, foreground);

		if (hit && _rigidBody.velocity.y == 0) {

			groundListener = hit.collider.GetComponent<RavenTouchedListener> ();
			if (groundListener != null) {
				groundListener.OnRavenTouchedBottom (this);
			}
			return true;
		} else if(groundListener != null){
			groundListener.OnRavenLeaving(this);
			groundListener = null;
		}

		return false;
	}

	private bool IsHittingHead ()
	{
		startLine.x = upperLeft.x + colliderSkin;
		startLine.y = upperLeft.y;
		endLine.x = upperRight.x - colliderSkin;
		endLine.y = upperRight.y;
		
		Debug.DrawLine (startLine, endLine);
		
		RaycastHit2D hit = Physics2D.Linecast (startLine, endLine, foreground);
		
		if (hit && _rigidBody.velocity.y == 0) {
			
			topListener = hit.collider.GetComponent<RavenTouchedListener> ();
			if (topListener != null) {
				topListener.OnRavenTouchedTop (this);
			}
			return true;
		} else if(topListener != null){
			topListener.OnRavenLeaving(this);
			topListener = null;
		}
		
		return false;
	}

	private bool IsOnRightWall ()
	{
		startLine.x = upperRight.x ;
		startLine.y = upperRight.y - colliderSkin;
		endLine.x = lowerRight.x ;
		endLine.y = lowerRight.y + colliderSkin;
		
		Debug.DrawLine (startLine, endLine);
		
		RaycastHit2D hit = Physics2D.Linecast (startLine, endLine, foreground);
		
		if (hit && _rigidBody.velocity.x == 0) {
			
			rightListener = hit.collider.GetComponent<RavenTouchedListener> ();
			if (rightListener != null) {
				rightListener.OnRavenTouchedRight (this);
			}
			return true;
		} else if(rightListener != null){
			rightListener.OnRavenLeaving(this);
			rightListener = null;
		}
		
		return false;
	}

	private bool IsOnLeftWall ()
	{
		startLine.x = upperLeft.x ;
		startLine.y = upperLeft.y - colliderSkin;
		endLine.x = lowerLeft.x ;
		endLine.y = lowerLeft.y + colliderSkin;
		
		Debug.DrawLine (startLine, endLine);
		
		RaycastHit2D hit = Physics2D.Linecast (startLine, endLine, foreground);
		
		if (hit && _rigidBody.velocity.x == 0) {
			
			leftListener = hit.collider.GetComponent<RavenTouchedListener>();
			if(leftListener != null)
			{
				leftListener.OnRavenTouchedLeft(this);
			} else if(leftListener != null){
				leftListener.OnRavenLeaving(this);
				leftListener = null;
			}
			return true;
		}
		
		return false;
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

	public void AddExternalVelocity(Vector2 extVelocity){
		this.externalVelocity = extVelocity;
	}

	public interface RavenTouchedListener{
		void OnRavenTouchedLeft(RavenController raven);
		void OnRavenTouchedRight(RavenController raven);
		void OnRavenTouchedTop(RavenController raven);
		void OnRavenTouchedBottom(RavenController raven);
		void OnRavenLeaving(RavenController raven);
	}
}
