using UnityEngine;
using System.Collections;

public class PlayerAnimationHandler {

	public int current_state = 0;
	private Animator anim;
	private PlayerValues values;
	private readonly int IDLEHASH = Animator.StringToHash ("Base Layer.idle");

	public PlayerAnimationHandler(Animator a, PlayerValues val)
	{
		anim = a;
		values = val;
	}


	public int getCurrentAnimationHashName(){

		return anim.GetCurrentAnimatorStateInfo (0).nameHash;

	}

	public bool isIdle()
	{
		return anim.GetCurrentAnimatorStateInfo (0).nameHash == IDLEHASH;
	}

	public void returnToIdle()
	{
		current_state = 0;
		anim.SetInteger ("current_state", current_state);
	}


	public void updateAnimation()
	{
//		if (isIdle())
//		{
			
			if (values.getAttack() == PlayerValues.inputState.HitMain) {


				if(!isRight() && isUp()){
					current_state = 1;
				}
				else if(!isRight() && !isUp()){
					current_state = 2;
				}
				else if(isRight() && !isUp()){
					current_state = 3;
				}
				else if(isRight() && isUp()){
					current_state = 4;
				}

				anim.SetInteger ("current_state", current_state);
				
				
			}
//		}
	}

	private bool isRight()
	{
		return values.getDirectionalX() == PlayerValues.inputState.WalkRight ||
						values.getDirectionalX() == PlayerValues.inputState.RunRight ||
						values.getFacing() == PlayerValues.facing.Right;
				
	}
	private bool isUp()
	{
		return values.getDirectionalY () == PlayerValues.inputState.Float||
			values.getDirectionalY () == PlayerValues.inputState.None;
	}

	
	public void endAttack()
	{
		anim.SetInteger ("current_state", 0) ;

	}
}
