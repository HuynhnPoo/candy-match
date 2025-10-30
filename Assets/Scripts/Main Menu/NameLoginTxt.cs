using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameLoginTxt :TextBase
{
    protected override void PrintText()
    {
        Debug.Log("ten login cua la"+ GameManager.Instance.NameUserLogin);
        text.SetText(GameManager.Instance.NameUserLogin);
    }

    
}
