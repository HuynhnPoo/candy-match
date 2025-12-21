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

        if (Input.GetKeyUp(KeyCode.N))
        {

            // IteminventoryManager.AddItem(itemData[0]);
            // IteminventoryManager.PrintInventory();

            /*GameMechanics.AddScore(5);
            GameMechanics.CheckHightScore(GameManager.Instance.Score);
            GameMechanics.CalculateMoney(5, 9, 7);

            Debug.Log($"tien kiem đượcb {GameManager.Instance.Coin} {GameManager.Instance.TotalCoin}");
            Debug.Log($" điểm đã chơi được {GameManager.Instance.Score} điểm cao nhất {GameManager.Instance.HighScore =PlayerPrefs.GetInt(StringManager.highScoreStr)}");

        */

            //Debug.Log("hien thi ra vang dax tieu"+PlayerPrefs.GetInt(StringManager.coinSaveStr));

            //IteminventoryManager.AddItem(itemData[0]);
            //IteminventoryManager.AddItem(itemData[1]);


        }


        if (Input.GetKeyUp(KeyCode.A))
        {

            /* 
             IteminventoryManager.UseItem("BoostActiveHammer");*/
            // DatabaseFirebaseManager.Instance.UpLoadCoinAndScore(GameManager.Instance.Coin, GameManager.Instance.HighScore);

            /* PlayerPrefs.DeleteKey(StringManager.coinSaveStr);
             PlayerPrefs.DeleteKey(StringManager.highScoreStr);*/

            // IteminventoryManager.PrintInventory();

        }



        if (Input.GetKeyUp(KeyCode.B))
        {
            int index = Random.Range(0, itemData.Length);
            IteminventoryManager.RemoveItem(itemData[index]);
        }
    }


    
}
