using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverOrWinTxt : TextBase
{
    protected override void PrintText()
    {
        text.SetText(GameManager.Instance.StatusGameStr);
    }
}
