using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : ButtonBase
{
    public override void OnClick()
    {
        UIManager.Instance.shopCanvas.SetActive(true);
    }

}
