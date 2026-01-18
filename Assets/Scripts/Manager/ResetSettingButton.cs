using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSettingButton : ButtonBase
{
    public override void OnClick()
    {
        SoundManager.Instance.ResetMusicAll();
    }
}
