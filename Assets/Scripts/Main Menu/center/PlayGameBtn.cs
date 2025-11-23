using System;
using UnityEngine;

public class PlayGameBtn : ButtonBase
{
    public override void OnClick()
    {
        BuyItemBuff();
        UIManager.Instance.ChangeScene(UIManager.SceneType.LEVELMENU);


        foreach (string nameItem in StringManager.itemBoosts)
        {
            IteminventoryManager.UseItem(nameItem);
        }

    }
    static void BuyItemBuff()
    {
        GameManager.Instance.OnboostGame = IncreaseTime;
        GameManager.Instance.OnboostGame += IncreaseTurn;
    }

    static void IncreaseTurn(string nameItem)
    {
        if (nameItem == StringManager.itemBoosts[0])// 0 đại dienj cho tăng lượt di chuyển
          Debug.Log ("thuc hien tang luowtj di chuyen");
    }
    static void IncreaseTime(string nameItem)
    {
        if (nameItem == StringManager.itemBoosts[1]) // 1 đại diện ttang thoi gian troi
            Debug.Log("thuc hien tang thời gian chơi");
    }


}
