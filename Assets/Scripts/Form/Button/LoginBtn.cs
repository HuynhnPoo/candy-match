using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginBtn : ButtonBase
{
    private FormHander hander ;
    public override void OnClick()
    {
        hander = UIManager.Instance.managerCanvas.GetComponentInChildren<FormHander>();

        hander?.Login(); // kiem tra login
    }

}
