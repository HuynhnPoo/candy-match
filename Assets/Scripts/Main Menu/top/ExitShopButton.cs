using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShopButton : ButtonBase
{
    public Ease Ease = Ease.Flash;
    public override void OnClick()
    {
        DatabaseFirebaseManager.Instance?.UpLoadCoinAndScore(GameManager.Instance.Coin,GameManager.Instance.HighScore);

        UIManager.Instance.shopCanvas.transform.GetChild(0).DOKill();
        UIManager.Instance.shopCanvas.transform.GetChild(0).DOScale(Vector3.zero, 0.5f).SetEase(Ease) //thuc hien tween
            .OnComplete(() => UIManager.Instance.shopCanvas.SetActive(false));// khi hoàn thanh sẽ tắt di 



    }
}
