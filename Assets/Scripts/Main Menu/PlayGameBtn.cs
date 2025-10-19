using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameBtn : ButtonBase
{
    public override void OnClick()
    {
        UIManager.Instance.CurrentScene = UIManager.SceneType.MAINMENU;
        UIManager.Instance.ChangeScene(UIManager.SceneType.LOADING);
    }

}
