using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShopButton : ButtonBase
{
    public override void OnClick()
    {
        DatabaseFirebaseManager.Instance.UpLoadCoinAndScore(GameManager.Instance.Coin,GameManager.Instance.HighScore);

        UIManager.Instance.shopCanvas.SetActive(false);
    }
}
