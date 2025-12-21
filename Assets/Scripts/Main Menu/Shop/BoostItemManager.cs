using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoostItemManager
{
  public static void HanldeItemUsed(TypeItem typeItem, string nameItem)
    {
        switch (typeItem)
        {
            case TypeItem.BUFF:
                Debug.Log("hien thuc hien buff");
                GameManager.Instance.OnboostGame?.Invoke(nameItem);
                break;
            case TypeItem.ACTIVE:
                Debug.Log("hien thuc hien active");
               
                GameManager.Instance.OnboostGame?.Invoke(nameItem);
                break;
            default:
                break;
        }
    }

   public static void InitializeBoost() 
    {

        GameManager.Instance.OnboostGame = IncreaseTime;
        GameManager.Instance.OnboostGame += IncreaseTurn;
    }
   
   static  void IncreaseTurn(string nameItem)
    {
        if (nameItem == StringManager.itemBoosts[0])// 0 đại dienj cho tăng lượt di chuyển
            Debug.Log("thuc hien tang luowtj di chuyen");
    }
    static void IncreaseTime(string nameItem)
    {
        if (nameItem == StringManager.itemBoosts[1]) // 1 đại diện ttang thoi gian troi
            Debug.Log("thuc hien tang thời gian chơi");
    }


    //============================
    public static void InitializeActive() 
    {

        GameManager.Instance.OnboostGame = ActiveHamer;
        GameManager.Instance.OnboostGame += ActiveShuffle;
    }

  static void ActiveHamer(string nameItem)
    {
        Debug.Log(nameItem);
        if (nameItem == "BoostActiveHammer")
        {

            Debug.Log("hien thi ra xoa hang candy");
            GameManager.Instance.IsClearCandy = true;
        }
    }

   static void ActiveShuffle(string nameItem)
    {
        Debug.Log(nameItem);
        if (nameItem == "BoostActiveShuffle")
        {

            Debug.Log("hien thi ra hoan doi candy ");
            GameManager.Instance.ShuffleCandyBoost();
        }
    }
}
