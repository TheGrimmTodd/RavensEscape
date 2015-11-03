using UnityEngine;
using System.Collections;

public class InplutHandlerKeyboard {

	public PlayerValues playerVal;
	private float timerR = -1;
	private float timerL = -1;
	private float timeTellRun = 2f;

	public InplutHandlerKeyboard(PlayerValues val)
	{
		playerVal = val;
	}
	
	public void updateInput()
	{
		
		// move left
		if (Input.GetKey (KeyCode.LeftArrow)) 
		{ 
			playerVal.setDirectionalX(PlayerValues.inputState.WalkLeft);
			playerVal.setFacing(PlayerValues.facing.Left);
			timerR = -1;
			if (timerL == -1) {
				timerL = Time.time;
			} else if (Time.time > timerL + timeTellRun) {
				//run
				playerVal.setDirectionalX(PlayerValues.inputState.RunLeft);
			}
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			playerVal.setDirectionalX(PlayerValues.inputState.WalkRight);
			playerVal.setFacing(PlayerValues.facing.Right);
			timerL = -1;
			if (timerR == -1) {
				timerR = Time.time;
			} else if (Time.time > timerR + timeTellRun) {
				//run
				playerVal.setDirectionalX(PlayerValues.inputState.RunRight);
			}
			
		} else {
			timerL = -1;
			timerR = -1;
			playerVal.setDirectionalX(PlayerValues.inputState.None);
		}
		
		//float or fall now is the time 
		if (Input.GetKey (KeyCode.DownArrow)) {
			playerVal.setDirectionalY(PlayerValues.inputState.Fall);
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			playerVal.setDirectionalY(PlayerValues.inputState.Float);
		} else {
			playerVal.setDirectionalY(PlayerValues.inputState.None);
		}
		
		// jump
		if (Input.GetKeyDown (KeyCode.Space)) { 
			playerVal.setJumps(PlayerValues.inputState.Jump);
		} else {
			playerVal.setJumps(PlayerValues.inputState.None);
		}
		
		if (Input.GetKeyDown (KeyCode.F)) {
			playerVal.setAttack(PlayerValues.inputState.HitMain);
		} else if (Input.GetKey (KeyCode.D)) {

		} else {
			playerVal.setAttack(PlayerValues.inputState.None);
		}
		

	}



}
