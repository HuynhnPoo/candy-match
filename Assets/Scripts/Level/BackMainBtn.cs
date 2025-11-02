using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMainBtn : ButtonBase
{
    public override void OnClick()
    {
        UIManager.Instance.ChangeScene(UIManager.SceneType.MAINMENU);
    }

    
}
