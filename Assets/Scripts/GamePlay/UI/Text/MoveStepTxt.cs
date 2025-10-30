using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStepTxt : TextBase
{
    protected override void PrintText()
    {
        text.SetText(GameManager.Instance.MoveStep.ToString());
    }

  
}
