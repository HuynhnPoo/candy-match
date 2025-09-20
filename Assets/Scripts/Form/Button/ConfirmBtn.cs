using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmBtn : ButtonBase
{
    private ConfirmFormHander hander;
    public override void OnClick()
    {
        hander = UIManager.Instance.managerCanvas.GetComponentInChildren<ConfirmFormHander>();

        Debug.Log("hien thi cos hander" + hander.name);

        hander?.ForgotPass();
    }
}
