using UnityEngine;
using System.Collections.Generic;

public class PlayerControler : MonoBehaviour {




	private PlayerAnimationHandler animationHandler;
	private InplutHandlerKeyboard inputHandler;
	private ScreanInput screeninput;
	private PlayerValues values;


	protected Transform _transform;
	protected Rigidbody2D _rigidbody;


	// raycast stuff
	private RaycastHit2D hit;
	private Vector2 physVel = new Vector2();
	private Transform groundCheck;




	public virtual void Awake()
	{
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
		values = new PlayerValues ();

	}


	// Use this for initialization
	void Start () {
		groundCheck = transform.Find("GroundCheck");
		animationHandler = new PlayerAnimationHandler (GetComponent<Animator>(), values);
//		inputHandler = new InplutHandlerKeyboard (values);
		screeninput = new ScreanInput (values);//ScreanInput (values,toutchInfo);

	}

	// Update is called once per frame
	void Update () {
		screeninput.updateInput ();
		//inputHandler.updateInput ();

		move ();
		animationHandler.updateAnimation ();

	}

	private void move(){

		physVel.x = getMoveVel (values.getDirectionalX());
		float increasedFall = getIncreasedFallVell(values.getDirectionalY());
		values.grounded = checkGrounded();

		if (values.grounded) {
			values.jumps = 0;
		}else{
			_rigidbody.AddForce(-Vector3.up * (values.fallVel));
		}
		// jumpS
		if(values.getJumps() == PlayerValues.inputState.Jump)
		{
			if(values.jumps < values.maxJumps)
			{
				values.jumps += 1;
				_rigidbody.velocity = new Vector2(physVel.x, values.jumpVel);
				
			}
		}

		// actually move the player
		_rigidbody.velocity = new Vector2(physVel.x, _rigidbody.velocity.y*increasedFall);


	}


	/***LOGIC METHODS ***/

	private float getMoveVel(PlayerValues.inputState currentInputState){

		switch (currentInputState) {
			
		case PlayerValues.inputState.WalkLeft:{
			return -values.walkVel;
		}
			
		case PlayerValues.inputState.WalkRight:{
			return values.walkVel;
		}
			
		case PlayerValues.inputState.RunLeft:{
			return -values.runVel;
		}
			
		case PlayerValues.inputState.RunRight:{
			return values.runVel;
		}
		case PlayerValues.inputState.None:
		default:
			return 0f;
	
		}


	}

	private float getIncreasedFallVell(PlayerValues.inputState currentInputState){
		
		if (_rigidbody.velocity.y < 0) 
		{
			if (currentInputState == PlayerValues.inputState.Fall) {
					return 1.02f;
			} else if (currentInputState == PlayerValues.inputState.Float) {
					return  .99f;
			} else {
					return 1f;
			}	
		}
		else
		{
			if (currentInputState == PlayerValues.inputState.Fall) {
				return .98f;
			} else if (currentInputState == PlayerValues.inputState.Float) {
				return  1.02f;
			} else {
				return 1f;
			}
		}
	}

	private bool checkGrounded()
	{
		return Physics2D.Linecast (_transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground"))
			||Physics2D.Linecast (_transform.position, new Vector2(groundCheck.position.x-.16f,groundCheck.position.y), 1 << LayerMask.NameToLayer("ground"))
				||Physics2D.Linecast (_transform.position, new Vector2(groundCheck.position.x+.16f,groundCheck.position.y), 1 << LayerMask.NameToLayer("ground"))	;
	}

	public void endAttack()
	{
		animationHandler.returnToIdle ();
	}

	void Respond ()
	{
		this.transform.position = new Vector2 (0f, 3f);
		this._rigidbody.velocity =   new Vector2 (0f, 0f);
	}

	public void kill()
	{	
		values.setLives (values.getLives() - 1);
//		if (values.getLives () > 0) {
				Respond ();
//		} 
//		else 
//		{
//			//checkIfEndGame
//		}
	}
	


}
