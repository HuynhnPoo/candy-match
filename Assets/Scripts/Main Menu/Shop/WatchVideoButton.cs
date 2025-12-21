using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchVideoButton : ButtonBase
{
    public override void OnClick()
    {
        LevelPlaySample.Instance.ShowRewardedAd();

    }

   
}
