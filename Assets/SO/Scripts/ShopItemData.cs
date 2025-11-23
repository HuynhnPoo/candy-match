using System;
using UnityEngine;
public enum TypeItem
{
    NONE,
    BUFF,
    ACTIVE
}
[CreateAssetMenu(fileName = "Itemdata", menuName = "Shop Item")]
public class ShopItemData : ScriptableObject
{


    public TypeItem typeItem = TypeItem.NONE;

    public string nameItem; // tên item
    public int priceItem; // giâ tien item
    public int valueBoostItem; //chỉ so gia trị tang
    public int quatityItem;// sô lương item
    public int maxQuatityItem;

    public Action<int> OnQuatityChanged;
    public int QuatityItem
    {
        get => quatityItem;
        set
        {
            if (quatityItem != value) { quatityItem = value; OnQuatityChanged?.Invoke(quatityItem); }
        }
    }

   
}
