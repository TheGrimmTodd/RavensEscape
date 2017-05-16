﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : AbstractActionable
{
    public RayLighting[] lightArray;
    public SwitchType switchType;
    [Range(-1,20)]
    public float timerLength;
    private float switchTime = 0;
    private bool switchState = true;

    void Update()
    {
        if(switchType.Equals(SwitchType.Timer) && switchTime != 0 && Time.time >= switchTime)
        {
            FlipSwitch();
            switchTime = 0;
        }
    }

    internal override void DoAction()
    {

        switch (switchType)
        {
            case SwitchType.OnOff:
                FlipSwitch();
                break;
            case SwitchType.Timer:
                if (Time.time >= switchTime)
                {
                    FlipSwitch();
                }
                switchTime = Time.time + timerLength;
                break;
            case SwitchType.OneOff:
                if(timerLength >= 0)
                {
                    FlipSwitch();
                }
                timerLength = -1;
                break;
        }
    }

    private void FlipSwitch()
    {
        switchState = !switchState;

        foreach (RayLighting light in lightArray)
        {
            light.TurnOn(switchState);
        }
    }

    public enum SwitchType
    {
        OnOff,
        Timer,
        OneOff
    }
}
