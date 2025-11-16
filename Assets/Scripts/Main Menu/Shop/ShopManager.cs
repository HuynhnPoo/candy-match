using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.M)) 
        {
            BuyItem();
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            // BuyItem();
            GameManager.Instance.OnboostGame = ActiveHamer;
            GameManager.Instance.OnboostGame += ActiveShuffle;
        }
    }



    public void BuyItem()
    {
        GameManager.Instance.OnboostGame = IncreaseTime;
        GameManager.Instance.OnboostGame += IncreaseTurn;
    }

    void IncreaseTurn(string nameItem)
    {
        if (nameItem == "BoostTurn")
            Debug.Log("thuc hien tang luowtj di chuyen");
    }
    void IncreaseTime(string nameItem)
    {
        if (nameItem == "BoostTime")
            Debug.Log("thuc hien tang thời gian chơi");
    }


    void ActiveHamer(string nameItem)
    {
        if (nameItem == "BoostActiveHamer")
            Debug.Log("hien thi ra thoi gian su dung bua");
    }

    void ActiveShuffle(string nameItem)
    {
        if (nameItem == "BoostActiveShuffle")
            Debug.Log("hien thi ra thoi gian su dung bua");
    }

}
