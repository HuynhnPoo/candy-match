using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuatityItemTxt : TextBase
{
    
    [SerializeField] private string nameItem;
    protected override void PrintText()
    {
        text.SetText("x" + IteminventoryManager.GetItemCount(nameItem));
    }
}
