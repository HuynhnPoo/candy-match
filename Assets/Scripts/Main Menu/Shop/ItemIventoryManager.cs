using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInfo
{
    public string NameItem;

    public int QuatityItem;
    public TypeItem TypeItem;

    public ItemInfo(string NameItem, int QuatityItem, TypeItem TypeItem)
    {
        this.NameItem = NameItem;
        this.QuatityItem = QuatityItem;
        this.TypeItem = TypeItem;
    }
}

[System.Serializable]
public class Iteminventory
{
    public List<ItemInfo> items = new List<ItemInfo>();
}

public class IteminventoryManager
{

    
    public static void PrintInventory()
    {
        // 1. Tải Inventory hiện tại vào Dictionary
        Dictionary<string, ItemInfo> inventory = Loadinventory();


        if (inventory.Count == 0)
        {
            Debug.Log("Inventory hiện tại RỖNG.");
            Debug.Log("===============================================");
            return;
        }

        int index = 1;

        // 2. Duyệt qua từng cặp Key-Value trong Dictionary
        foreach (KeyValuePair<string, ItemInfo> item in inventory)
        {
            string potionName = item.Key;
            ItemInfo entry = item.Value;

            // 3. In thông tin chi tiết của mỗi PotionEntry
            Debug.Log($"[{index}] Tên: {potionName} | SL: {entry.QuatityItem} | Loại: {entry.TypeItem} | Value: {entry.NameItem}");

            index++;
        }
        Debug.Log($"================ Tổng cộng: {inventory.Count} loại vật phẩm ==================");
    }
    private static Dictionary<string, ItemInfo> Loadinventory()
    {
        Dictionary<string, ItemInfo> inventory = new Dictionary<string, ItemInfo>();
        if (PlayerPrefs.HasKey(StringManager.saveBuyItem))
        {
            string json = PlayerPrefs.GetString(StringManager.saveBuyItem);  //láy các giá trị đã luuw bằng json
            Iteminventory item = JsonUtility.FromJson<Iteminventory>(json); // chuyển sang json và gắn cho item
            if (item != null && item.items != null)
            {
                foreach (ItemInfo itemInfo in item.items)
                {
                    if (!inventory.ContainsKey(itemInfo.NameItem)) // kiểm tra thêm vào trong danh sách
                        inventory.Add(itemInfo.NameItem, itemInfo);

                }

            }

        }
        return inventory; // trả vể danh sách item
    }


    static void SaveInventory(Dictionary<string, ItemInfo> inventory)
    {
        Iteminventory Iteminventory = new Iteminventory();
        foreach (var item in inventory.Values)
        {
            if (item.QuatityItem > 0)
                Iteminventory.items.Add(item);// them vào trong danh sách
        }
        string json = JsonUtility.ToJson(Iteminventory); //chuyerern itemiventory sang json thanh string
        PlayerPrefs.SetString(StringManager.saveBuyItem, json); // luu lại
        PlayerPrefs.Save();
    }

    public static void AddItem(ShopItemData itemData)
    {


        Dictionary<string, ItemInfo> invetory = Loadinventory();
        string key = itemData.nameItem;

        if (invetory.ContainsKey(key))
        {
            invetory[key].QuatityItem++; // nêu có rồi thi sẽ tăng quatity lên 
            itemData.quatityItem = invetory[key].QuatityItem;
            Debug.Log("so luong item"+itemData.quatityItem);
        }
        else
        {
            ItemInfo item = new ItemInfo(itemData.nameItem, 1, itemData.typeItem);
            invetory.Add(key, item);    // tạo và thêm vao trong inventory
        }

        SaveInventory(invetory);    //lưu dư liêu lại 


    }

    public static void UseItem(string nameItem)
    {
        
        Dictionary<string, ItemInfo> invetory = Loadinventory();

        if (invetory.ContainsKey(nameItem) && invetory[nameItem].QuatityItem > 0)
        {

            ItemInfo itemInfo = invetory[nameItem];

            invetory[nameItem].QuatityItem--;
            Debug.Log("hien thi "+ invetory[nameItem].QuatityItem);

            // ham thu hien boost cho game
            BoostItemManager.HanldeItemUsed(invetory[nameItem].TypeItem, nameItem);

            if (invetory[nameItem].QuatityItem <= 0) invetory.Remove(nameItem);
            SaveInventory(invetory);
        }
    }


   

    public static int GetItemCount(string nameItem)
    {
        Dictionary<string, ItemInfo> invetory = Loadinventory(); // load các item đã lưu
        if (invetory.ContainsKey(nameItem))
        {
          
            return invetory[nameItem].QuatityItem; // trả về số lượng đố
        }
        return 0;
    }

    public static void RemoveItem(ShopItemData itemData)  // ham xoa danh sachs
    {
        PlayerPrefs.DeleteKey(StringManager.saveBuyItem);
        PlayerPrefs.Save();
    }


}
