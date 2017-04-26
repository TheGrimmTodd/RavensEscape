using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : AbstractZone
{
    protected override Sides ZoneSides()
    {
        return new Sides(false, false, true, false);
    }

    protected override void OnEntered(Collider2D collider)
    {
        collider.GetComponent<Spawn>().PlayerFallen();
    }
}
