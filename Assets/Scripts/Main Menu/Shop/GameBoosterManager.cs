using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoosterManager : MonoBehaviour
{
   
    private void OnEnable()
    {
        IteminventoryManager.OnItemUsed += HanldeItemUsed;
    }
    private void OnDisable()
    {
        IteminventoryManager.OnItemUsed -= HanldeItemUsed;

    }
    // Start is called before the first frame update
    void Start()
    {
       
    }


    void HanldeItemUsed(TypeItem typeItem,string nameItem)
    {
        Debug.Log(typeItem +nameItem);
        switch (typeItem)
        {
            case TypeItem.BUFF:
                Debug.Log("hien thuc hien buff");
                GameManager.Instance.OnboostGame?.Invoke("BoostTurn");
                break;
            case TypeItem.ACTIVE:
                Debug.Log("hien thuc hien active");
                // actice dang có aaaa nên chưa dung hammer được chú ý
                GameManager.Instance.OnboostGame?.Invoke("BoostActiveHammer");
                break;
            default:
                break;
        }
    }
}
