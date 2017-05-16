using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipZone : AbstractZone {

    [Range(1,5)]
    public int showTipTimes = 3;
    public String toolTip;
    public LevelText levelText;

    private int timesShowed = 0;

    protected override void OnEntered(Collider2D collider)
    {
        if(timesShowed < showTipTimes)
        {
            showTip();
        }
    }

    protected override Sides ZoneSides()
    {
        bool look = timesShowed < showTipTimes;
        return new Sides(look, look, look, false);
    }

    private void showTip()
    {
        timesShowed++;
        levelText.SetLevelText(toolTip, LevelText.TextType.Short);
    }

}
