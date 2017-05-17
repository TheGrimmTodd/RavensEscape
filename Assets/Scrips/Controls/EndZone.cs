using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : AbstractZone
{
    protected override Sides ZoneSides()
    {
        return new Sides(true, true, true, true);
    }

    protected override void OnEntered(Collider2D collider)
    {
        collider.GetComponent<Spawn>().FinishedLevel();
    }
}
