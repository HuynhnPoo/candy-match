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


            //DataUser currentUser = DatabaseFirebaseManager.Instance.UserFound;
            //Debug.Log("hien" + currentUser.nameUser+ currentUser.id);

            GameMechanics.AddScore(5);
            GameMechanics.CheckHightScore(GameManager.Instance.Score);
            GameMechanics.CalculateMoney(5,9,7);
           // Debug.Log($"tien kiem đượcb {GameManager.Instance.Coin} {GameManager.Instance.TotalCoin}");
           //Debug.Log($" điểm đã chơi được {GameManager.Instance.Score} điểm cao nhất {GameManager.Instance.HighScore}");
            
        }
        
        if (Input.GetKeyUp(KeyCode.C))
        {
            GameManager.Instance.TotalCoin = PlayerPrefs.GetInt(StringManager.coinSaveStr);
            GameManager.Instance.HighScore = PlayerPrefs.GetInt(StringManager.highScoreStr);
           // Debug.Log($"tien kiem đượcb {GameManager.Instance.TotalCoin}");
            Debug.Log($" điểm cao nhất {GameManager.Instance.HighScore}");

        }

        if (Input.GetKeyUp(KeyCode.A))
        {

           /* GameManager.Instance.OnboostGame = ActiveHamer;
            GameManager.Instance.OnboostGame += ActiveShuffle;
            IteminventoryManager.UseItem("BoostActiveHammer");*/

           /* PlayerPrefs.DeleteKey(StringManager.coinSaveStr);
            PlayerPrefs.DeleteKey(StringManager.highScoreStr);*/

        } if (Input.GetKeyUp(KeyCode.B))
        {
            int index = Random.Range(0, itemData.Length);
            IteminventoryManager.RemoveItem(itemData[index]);
        }
    }


    void ActiveHamer(string nameItem)
    {
        Debug.Log(nameItem);
        if (nameItem == "BoostActiveHammer")
            Debug.Log("hien thi ra thoi gian su dung bua");
    }

    void ActiveShuffle(string nameItem)
    {
        if (nameItem == "BoostActiveShuffle")
            Debug.Log("hien thi ra thoi gian su dung bua");
    }
}
