using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameInput : InputBase
{
    public string nameID{ get; private set; } 
    protected override void OnEndEdit(string text)
    {
      this.nameID = text;
    }

   
}
