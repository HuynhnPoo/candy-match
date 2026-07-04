using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GameMechanics
{
    private static bool timeUp = false;

    private static float time;
    static float scoreMultiles = 0.1f;

    private static int score = 0;
    static int bonusRewads = 5;
    static int bonusRewadsUseBoost;
    public static void Init(float addTime, int addfloatbonusRewadsUseBoost)
    {
        score = 0;
        time = addTime;
        timeUp = false;
        bonusRewadsUseBoost = addfloatbonusRewadsUseBoost;
    }


    public static void AddScore(int amout)
    {
        if (amout == 3)
        {
            score += 5; // thêm điểm
        }
        else if (amout >= 4 && amout <= 6)
        {
            score += 8; // thêm điểm
        }
        else if (amout >= 7 && amout <= 8)
        {
            score += 10;
        }
        GameManager.Instance.Score = score;

    }



    public static float CountDown()
    {
        if (!timeUp)
        {
            time -= Time.deltaTime;
            //   Debug.Log("hien thi ra "+ time);
            if (time <= 0)
            {
                timeUp = true;
                time = 0;

                Debug.Log("het gio");

            }
        }
        return time;
    }

    public static bool CheckInernet()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    // ham tinh tinh toan gia trij kiem tien
    public static void CalculateMoney(int score, int time, int stepMove)
    {
        int coinEarned = ((score * (int)scoreMultiles) + (time * bonusRewads) + (stepMove * bonusRewads)) * bonusRewadsUseBoost;

        GameManager.Instance.Coin = coinEarned;

        GameManager.Instance.TotalCoin += GameManager.Instance.Coin;
        PlayerPrefs.SetInt(StringManager.coinSaveStr, GameManager.Instance.TotalCoin);
    }

    public static string ConvertFirebaseJsonToArray(string json)
    {
        if (string.IsNullOrWhiteSpace(json) || json == "null")
        {
            return "{ \"user\": [] }";
        }

        json = json.Trim();

        // đảm bảo bỏ {} ngoài cùng nếu có
        if (json.StartsWith("{") && json.EndsWith("}"))
        {
            json = json.Substring(1, json.Length - 2);
        }

        // pattern: "KEY" : { ... }   -- lấy KEY và phần nội dung {...}
        // RegexOptions.Singleline để '.' có thể match newline
        string pattern = "\"?(.*?)\"?\\s*:\\s*\\{(.*?)\\}(,|\\s*$)";
        var matches = Regex.Matches(json, pattern, RegexOptions.Singleline);

        List<string> objects = new List<string>();

        foreach (Match m in matches)
        {
            if (m.Groups.Count >= 3)
            {
                string key = m.Groups[1].Value.Trim().Trim('"');
                string inner = m.Groups[2].Value.Trim(); // nội dung bên trong {...}

                // Nếu inner rỗng thì dùng object trống
                if (string.IsNullOrEmpty(inner))
                    inner = "";

                // Nếu inner đã có id field (hiếm), thì không thêm nữa
                bool hasId = Regex.IsMatch(inner, "\"id\"\\s*:");

                string obj;
                if (!hasId)
                {
                    // chèn "id":"KEY" vào đầu object
                    if (string.IsNullOrEmpty(inner))
                        obj = $"{{\"id\":\"{EscapeJsonString(key)}\"}}";
                    else
                        obj = $"{{\"id\":\"{EscapeJsonString(key)}\",{inner}}}";
                }
                else
                {
                    // nếu đã có id thì giữ nguyên (nhưng vẫn wrap lại)
                    obj = "{" + inner + "}";
                }

                objects.Add(obj);
            }
        }

        string fixedJson = "{ \"user\": [" + string.Join(",", objects) + "] }";
        return fixedJson;
    }

    // Helper nhỏ để escape các kí tự trong id nếu cần
    private static string EscapeJsonString(string s)
    {
        if (s == null) return "";
        return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    public static void CheckHightScore(int score)
    {
        int currentScore = PlayerPrefs.GetInt(StringManager.highScoreStr);

        Debug.Log("hien thi score" + currentScore + " " + score);
        if (score > currentScore)
        {

            PlayerPrefs.SetInt(StringManager.highScoreStr, score);
            PlayerPrefs.Save();

            GameManager.Instance.HighScore = PlayerPrefs.GetInt(StringManager.highScoreStr);
        }

    }



    // ham mua
    public static void BuyItemChecking(Dictionary<string, ShopItemData> listItem, string nameItem)
    {
        if (listItem.TryGetValue(nameItem, out ShopItemData itemData))
        {
            if (GameManager.Instance.Coin >= itemData.priceItem && itemData.quatityItem < itemData.maxQuatityItem)
            {

                GameManager.Instance.Coin -= itemData.priceItem;
                PlayerPrefs.SetInt(StringManager.coinSaveStr, GameManager.Instance.Coin);

                PlayerPrefs.Save();
                GameManager.Instance.Coin = PlayerPrefs.GetInt(StringManager.coinSaveStr);
                //cho playpef xuongo add item
                IteminventoryManager.AddItem(itemData); // them vo trong danh sach sau khi quatityTxt


            }

            else
            {
                UIManager.Instance.ShowNotificationBuy();
            }


        }

    }


    // ham kieem tra theo chieuef dojc color bomb
    public static void CheckVerticalColorBomb(GridManager gridManager, CandyVisual[,] candies, HashSet<CandyVisual> match, Vector2Int verticalA, Vector2Int verticalB, int height)
    {
        int col = verticalA.y;

        int minVertical = Mathf.Min(verticalA.x, verticalB.x) - 1;
        int maxVertical = Mathf.Max(verticalA.x, verticalB.x) + 1;


        if (minVertical >= 0 && candies[minVertical, col] != null)
            Debug.Log("type cua candy " + candies[minVertical, col].TypeCandy);
        else if (maxVertical < height && candies[maxVertical, col] != null)
            Debug.Log("type cua candy " + candies[maxVertical, col].TypeCandy);

    }

    //ham kiểm tra chiều nagng color bomb
    public static void CheckHorizontalColorBomb(GridManager gridManager, CandyVisual[,] candies, HashSet<CandyVisual> match, Vector2Int horizontalA, Vector2Int horizontalB, int width)
    {
        int row = horizontalA.x;

        int minHorizontal = Mathf.Min(horizontalA.x, horizontalB.x) - 1;
        int maxHorizontal = Mathf.Max(horizontalA.x, horizontalB.x) + 1;

        if (minHorizontal >= 0 && candies[row, minHorizontal])
        {
            Debug.Log("hien thi ra type khi match horizontal" + candies[row, minHorizontal].TypeCandy);
        }

        else if (maxHorizontal < width && candies[row, minHorizontal])
        {
            Debug.Log("hien thi ra type khi match horizontal" + candies[row, minHorizontal].TypeCandy);
        }
    }

    // thuwjc hien nor theo hình dấu cộng
    static void ImplementExplorePlus(GridManager gridManager,CandyVisual[,] candies,HashSet<CandyVisual> match,int row,int col,int size) { }


    // thu hiên  di chuyên chinh xác 

    

}