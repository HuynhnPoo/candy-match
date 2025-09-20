using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterBtn : ButtonBase
{
    private FormHander hander;
    public override void OnClick()
    {
        hander = UIManager.Instance.managerCanvas.GetComponentInChildren<FormHander>();

        Debug.Log("hien thi cos hander" + hander.name);

        hander.Register();
    }
}
