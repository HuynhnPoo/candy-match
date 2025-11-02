using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelBtn : ButtonBase
{
    public override void OnClick()
    {
        Time.timeScale = 1;
       
        UIManager.Instance.gameoverPn.SetActive(false);
        UIManager.Instance.ChangeScene(UIManager.SceneType.LEVELMENU);
    }

}
