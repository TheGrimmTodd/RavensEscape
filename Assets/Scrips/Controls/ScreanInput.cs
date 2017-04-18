using UnityEngine;
using System.Collections;

public class ScreanInput {

	public GUIText gui;
	//swipe testing valse
	float rstartTime ;
	float maxSwipeTime = 0.34f; //.5f;
	float minSwipeTime = 0.03f;//.1f;
	float minSwipeDist = 70f;//20f;
	float maxSwipeDist = 470f;

	float nullarea = 35;
	int swipecount = 0;
	float avgTime = 0;
	float avgDist = 0;
//	string touchinfo = "";
	private Touch Right;
	private Touch Left;
	bool initLeft = false;
	bool initRight = false;
	
	int r = 0;
	int l = 0;

	private Vector2 LeftStart;
	private Vector2 RightStart;


	private int middle;

	public void updateInput()
	{
		middle =  Screen.currentResolution.width/2;

		r = 0; l = 0;
		foreach(Touch tut in Input.touches)
		{
			float x = tut.position.x;
			bool isRight = x > middle;


			if(isRight){
				r++;
	
				if(initRight)
				{

					if(Right.fingerId == tut.fingerId)
					{
			
						Right = tut;

						TouchPhase phase = Right.phase;
						//could be a swipe!!!
			
						if(phase == TouchPhase.Canceled ||
						   phase == TouchPhase.Ended)
						{

							float swipeTime = Time.time - rstartTime;			
							float swipeDist = (Right.position.y - RightStart.y);
							//calculateSwipeVals(  swipeDist, swipeTime);
							if(swipeTime > minSwipeTime &&
							   swipeTime < maxSwipeTime &&
							   swipeDist > minSwipeDist &&
							   swipeDist < maxSwipeDist)
							{
								//swipe
								
							}
							else
							{
								//tap
							}
						}
					
						//TODO check for holding for shield

					}
					else if(initLeft && Left.fingerId == tut.fingerId)
					{
						endLeft();
					}
					else
					{
						//drop this toutch it is an extra one we don't care about
					}
				}
				else if(!(initLeft && Left.fingerId == tut.fingerId))
				{
					Right = tut;
					initRight = true;
					RightStart = Right.position;
					rstartTime = Time.time;
				}
				else
				{
					endLeft();
				}

			}
			else
			{
				l++;
				if(initLeft)
				{
					if(Left.fingerId == tut.fingerId)
					{
						Left = tut;
					
						float ldeltx = Left.position.x - LeftStart.x;
						float ldelty = Left.position.y - LeftStart.y;
						float dist = Mathf.Abs(Vector2.Distance(Left.position , LeftStart));


						if(dist > nullarea)
						{

							if(ldeltx < -nullarea)
							{
								// left
							}
							else if(ldeltx > nullarea)
							{
								// right						
							}
							
							if(ldelty > nullarea )
							{
								// up
							}
							else if(ldelty < -nullarea )
							{
								//down
							}
						}
						else 
						{	
							//nothing

						}
					}
					else if(!(initRight && Right.fingerId == tut.fingerId))
					{
						endRight();
					}
					else
					{
						//drop this toutch it is an extra one we don't care about
					}
				}
				else if(!(initRight && Right.fingerId == tut.fingerId))
				{
					Left = tut;
					initLeft = true;
					LeftStart = Left.position;
				}
				else
				{
					endRight();
				}
			}
		}
		if (l == 0 || !initLeft) endLeft ();
		if (r == 0 || !initRight) endRight ();
	}

	private void endRight()
	{
		initRight = false;
		
	}
	private void endLeft()
	{
		initLeft = false;
	}

	private void calculateSwipeVals( float swipeDist,float swipeTime)
	{
		swipecount++;
		minSwipeDist = swipeDist < minSwipeDist? swipeDist: minSwipeDist;
		maxSwipeDist = swipeDist > maxSwipeDist? swipeDist: maxSwipeDist;
		minSwipeTime = swipeTime < minSwipeTime? swipeTime: minSwipeTime;
		maxSwipeTime = swipeTime > maxSwipeTime? swipeTime: maxSwipeTime;
		
		avgDist = ((swipecount - 1)/swipecount) * avgDist + swipeDist * (1/swipecount);
		avgTime = ((swipecount - 1)/swipecount) * avgTime + swipeTime * (1/swipecount);
//		touchinfo = "minSwipeDist: " + minSwipeDist + "\n"
//			+"maxSwipeDist: " + maxSwipeDist + "\n"
//				+"minSwipeTime: " + minSwipeTime + "\n"
//				+"maxSwipeTime: " + maxSwipeTime + "\n"
//				+"avgDist: " + avgDist + "\n"
//				+"avgTime: " + avgTime + "\n";
	}
}
