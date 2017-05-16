using UnityEngine;
using System.Collections;
using System;

public class PlayerController : RaycastController
{
    public CollisionInfo collisions;
    public LayerMask actionableMask;

    [HideInInspector]
    public Vector2 playerInput;

    public override void Start()
    {
        base.Start();
        collisions.faceDir = 1;
    }

    public void Move(Vector2 moveAmount, bool standingOnPlatform = false)
    {
        Move(moveAmount, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector2 moveAmount,Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.moveAmountOld = moveAmount;
        playerInput = input;

        if(moveAmount.x != 0)
        {
            collisions.faceDir = (int) Mathf.Sign(moveAmount.x);
        }
        HorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }

        transform.Translate(moveAmount);

        if (standingOnPlatform)
        {
            collisions.below = true;
        }
    }

    void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if(Mathf.Abs(moveAmount.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }
                moveAmount.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    internal void CheckForActionable()
    {

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, collider.bounds.size.y - skinWidth * 2, actionableMask);
            if (hit)
            {
                hit.transform.GetComponent<AbstractActionable>().DoAction();
                return;
            }
            rayOrigin = raycastOrigins.bottomLeft + Vector2.right * (verticalRaySpacing * i);
            hit = Physics2D.Raycast(rayOrigin, Vector2.up, collider.bounds.size.y - skinWidth * 2, actionableMask);
            if (hit)
            {
                hit.transform.GetComponent<AbstractActionable>().DoAction();
                return;
            }
        }
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft + Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, collider.bounds.size.x - skinWidth * 2, actionableMask);
            if (hit)
            {
                hit.transform.GetComponent<AbstractActionable>().DoAction();
                return;
            }
            rayOrigin = raycastOrigins.bottomRight + Vector2.up * (horizontalRaySpacing * i);
            hit = Physics2D.Raycast(rayOrigin, Vector2.left, collider.bounds.size.x - skinWidth * 2, actionableMask);
            if (hit)
            {
                hit.transform.GetComponent<AbstractActionable>().DoAction();
                return;
            }
        }
    }

    void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public Vector2 moveAmountOld;
        public int faceDir;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

    public void SeenByLight()
    {
        transform.position = Vector3.zero + Vector3.forward * -10; 
    }
}