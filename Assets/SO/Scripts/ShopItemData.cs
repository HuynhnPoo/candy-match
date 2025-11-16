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


}
