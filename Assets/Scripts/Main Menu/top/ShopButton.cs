using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : ButtonBase
{
    public Ease Ease =Ease.Flash;
    public override void OnClick()
    {
        UIManager.Instance.shopCanvas.SetActive(true);
    }

}
