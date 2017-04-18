using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

    float moveSpeed = 6;
    float timeToJumpApex = .4f;
    float maxJumpHeight = 4;
    float minJumpHeight = 1;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallJumpLeap;
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;

    Vector3 velocity;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float velocityXSmothing;
    float timeToWallUnstick;
    Vector2 directionInput;
    bool wallSliding;
    float wallDirX;

    PlayerController controller;

	void Start () {
        controller = GetComponent <PlayerController>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

    void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }

    public void SetDirectionalInput (Vector2 input)
    {
        directionInput = input;
    }
	
    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallJumpLeap.x;
                velocity.y = wallJumpLeap.y;

            }
        }
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;

        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;
            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmothing = 0;
                velocity.x = 0;

                if (directionInput.x != wallDirX && directionInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x,
                                      targetVelocityX,
                                      ref velocityXSmothing,
                                      (controller.collisions.below
                                            ? accelerationTimeGrounded
                                            : accelerationTimeAirborne));
        velocity.y += gravity * Time.deltaTime;
    }
}
