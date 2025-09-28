using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTxt :TextBase
{
    protected override void PrintText()
    {
        this.text.SetText("Score: "+GameManager.Instance.Score.ToString());
    }

 
}
