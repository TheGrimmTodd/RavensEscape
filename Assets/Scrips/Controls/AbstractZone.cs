using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractZone : RaycastController
{
    public bool debugLines;
    [Range(1, 10)]
    public float zoneWidth = 4;
    private float zoneHight = 4;
    private bool entered;

    public override void Start()
    {
        entered = false;
        transform.localScale = new Vector2(zoneWidth, zoneHight);
        base.Start();
        UpdateRaycastOrigins();
    }

    void Update()
    {
        Sides sides = ZoneSides();
        if (sides.up || sides.down)
            for (int i = 0; i < verticalRayCount; i++)
            {
                if ((sides.up && LookUp(i)) || (sides.down && LookDown(i)))
                {
                    return;
                }
            }
        
        if(sides.left || sides.right)
            for (int i = 0; i < horizontalRayCount; i++)
            {
                if ((sides.left && LookLeft(i)) || (sides.right && LookRight(i)))
                {
                    return;
                }
            }
    }

    private bool LookUp(int i)
    {
        Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
        if(debugLines) Debug.DrawRay(rayOrigin, Vector2.up, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, 1, collisionMask);
        return HandleHit(hit);
    }
    private bool LookDown(int i)
    {
        Vector2 rayOrigin = raycastOrigins.bottomLeft + Vector2.right * (verticalRaySpacing * i);
        if(debugLines) Debug.DrawRay(rayOrigin, Vector2.down, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 1, collisionMask);
        return HandleHit(hit);
    }
    private bool LookLeft(int i)
    {
        Vector2 rayOrigin = raycastOrigins.bottomLeft + Vector2.up * (horizontalRaySpacing * i);
        if (debugLines) Debug.DrawRay(rayOrigin, Vector2.left, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, 1, collisionMask);
        return HandleHit(hit);
    }
    private bool LookRight(int i)
    {
        Vector2 rayOrigin = raycastOrigins.bottomRight + Vector2.up * (horizontalRaySpacing * i);
        if(debugLines) Debug.DrawRay(rayOrigin, Vector2.right, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, 1, collisionMask);
        return HandleHit(hit);
    }
    private bool HandleHit(RaycastHit2D hit)
    {
        if (!entered && hit && hit.distance == 0)
        {
            entered = true;
            OnEntered(hit.collider);
            return true;
            
        }else if (hit && hit.distance != 0)
        {
            entered = false;
            return true;
        }

        return false;
    }
    protected abstract Sides ZoneSides();
    protected abstract void OnEntered(Collider2D collider);

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector2(zoneWidth, zoneHight));
    }

    public struct Sides
    {
        public readonly bool left, right;
        public readonly bool up, down;
        public Sides(bool left, bool right, bool up, bool down)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
        }
    }
}
