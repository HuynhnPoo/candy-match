using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginBtn : ButtonBase
{
    private FormHander hander ;
    public override void OnClick()
    {
        hander = UIManager.Instance.managerCanvas.GetComponentInChildren<FormHander>();

        Debug.Log("hien thi cos hander"+hander.name);

        hander?.Login();
    }

    protected override void OnEnable()
    {
     
    }

}
