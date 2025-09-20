using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPassInput : InputBase
{

    public string comfirmPassword { get; private set; }
    protected override void OnEndEdit(string text)
    {
      this.comfirmPassword = text;
    }

   
}
