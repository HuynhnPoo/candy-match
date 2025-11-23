using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShopButton : ButtonBase
{
    public override void OnClick()
    {

        UIManager.Instance.shopCanvas.SetActive(false);
    }
}
