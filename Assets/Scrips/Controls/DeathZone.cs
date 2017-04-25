using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : RaycastController {

    public Vector2 size = new Vector2(2.5f, 2.5f);
    [Range(1, 10)]
    public float zoneWidth = 4;
    private float zoneHight = 4;

    public override void Start()
    {
        transform.localScale = new Vector2(zoneWidth, zoneHight);
        base.Start();
        UpdateRaycastOrigins();
    }

    void Update () {
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, 1, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up, Color.red);
            if (hit && hit.distance == 0)
            {
                Spawn spawner = hit.collider.transform.GetComponent<Spawn>();
                spawner.PlayerFallen();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector2(zoneWidth, zoneHight));
    }
}
