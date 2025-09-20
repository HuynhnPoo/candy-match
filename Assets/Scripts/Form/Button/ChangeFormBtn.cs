using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFormBtn : ButtonBase
{
    public override void OnClick()
    {
        Debug.Log("hien thi ra form thuc hien " + gameObject.name);
        ChangeForm();
    }

    void ChangeForm()
    {


        if (UIManager.Instance.forgotForm.activeSelf == true)
        {
            UIManager.Instance.loginForm.SetActive(true);
            UIManager.Instance.forgotForm.SetActive(false);

        }

        else if(UIManager.Instance.loginForm.activeSelf == true) 
        {
            UIManager.Instance.loginForm.SetActive(false);
            UIManager.Instance.forgotForm.SetActive(true);
        }


    }

   
}
