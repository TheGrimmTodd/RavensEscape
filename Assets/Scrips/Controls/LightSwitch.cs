using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Switch
{
    public RayLighting[] lightArray;

    protected override void SwitchFlipped(bool switchState)
    {
        foreach (RayLighting light in lightArray)
        {
            light.TurnOn(switchState);
        }
    }
}
