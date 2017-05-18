using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitch : Switch {

    public PlatformController controller;

    protected override void SwitchFlipped(bool switchState)
    {
        controller.CanMove(switchState);
    }
}
