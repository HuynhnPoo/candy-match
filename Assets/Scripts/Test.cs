using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private ShopItemData[] itemData;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.V))
        {
            int index = Random.Range(0, itemData.Length);
            IteminventoryManager.AddItem(itemData[index]);
        }
        if (Input.GetKeyUp(KeyCode.N))
        {

            IteminventoryManager.PrintInventory();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            IteminventoryManager.UseItem("aaaaaaaaaa");
        } if (Input.GetKeyUp(KeyCode.B))
        {
            int index = Random.Range(0, itemData.Length);
            IteminventoryManager.RemoveItem(itemData[index]);
        }
    }
}
