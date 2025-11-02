using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GameMechanics
{
    private static int score = 0;

    private static float time = 30;
    private static bool timeUp = false;
    static float scoreMultiles = 0.1f;
    static int bonusRewads = 5;
    public static void Init()
    {
        score = 0;
        time = 30;
        timeUp = false;
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
        Debug.Log("diemrd cua game là " + score);
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

    public static void CalculateMoney(int score, int time,int stepMove)
    {
        int coinEarned = (score*(int)scoreMultiles)+(time*bonusRewads)+(stepMove*bonusRewads);
        GameManager.Instance.Coin= coinEarned;
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
}