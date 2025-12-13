using System;
using UnityEngine;

public class PlayGameBtn : ButtonBase
{
    public override void OnClick()
    {
        BoostItemManager.InitializeBoost();
        UIManager.Instance.ChangeScene(UIManager.SceneType.LEVELMENU);


        foreach (string nameItem in StringManager.itemBoosts)
        {
            IteminventoryManager.UseItem(nameItem);
        }

    }

}
