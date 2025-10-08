#if !UNITY_WEBGL
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DatabaseFirebaseManager : SingletonBase<DatabaseFirebaseManager>
{
#if !UNITY_WEBGL
    private DatabaseReference dataRef; // biên firebase danh cho pc và mobie
#endif
    private string dataURL = "https://saga-candy-default-rtdb.asia-southeast1.firebasedatabase.app/";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // compoment cho scene
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //cac cooment cho scene
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name != "BOOTSTRAP")
        {
            Init();
        }
    }

    void Init()
    {
#if !UNITY_WEBGL
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {

                FirebaseApp app = FirebaseApp.DefaultInstance;
                dataRef = FirebaseDatabase.DefaultInstance.RootReference;

                Debug.Log("khơi tạo thành công firebase");
                //
                UpLoadAllData();// tải tất cả dữ liệu lên trên fire base khi bắt đầu game
                                // ReadDataOption("huynh0");
            }
            else
                Debug.LogWarning("khởi tạo  firebase không thành công" + task.Exception);

        });
#else
        UpLoadAllData();

#endif
    }


    void UpLoadAllData()
    {
        UserList user = new UserList().LoadUsers(); // load dữ liệu ở trong json
        foreach (var item in user.user)
        {
            WriteDataOption(item);

        }
    }

    public void WriteDataOption(DataUser user)
    {
#if UNITY_WEBGL // khi build ra wed
        StartCoroutine(PutDataCoroutine(user, (result) =>
        {
            Debug.Log("WebGL WriteData OK: " + result);
        }));
#else //khi build ra pc va mb
        // PC/Mobile: dùng Firebase SDK
        string jsonUser = JsonUtility.ToJson(user);
        dataRef.Child("Users").Child(user.id).SetRawJsonValueAsync(jsonUser).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Firebase SDK ghi dữ liệu thành công");
            }
            else
            {
                Debug.LogError("Firebase SDK lỗi: " + task.Exception);
            }
        });
#endif
    }

    public void ReadDataOption(string user, string password = "", Action<bool> onComplete = null)
    {

#if UNITY_WEBGL //su dung khi build ra wed
        StartCoroutine(GetDataCoroutine(user, password, (result) =>
        {
            Debug.Log("WebGL Read Data OK: " + result);
            if (!string.IsNullOrEmpty(result))
                onComplete?.Invoke(true);
            else onComplete?.Invoke(false);
        }));
#else // su dung khi build ra wed
        dataRef.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {

            if (task.IsCompleted)
            {

                DataSnapshot dataSnap = task.Result;
                if (dataSnap.Exists)
                {
                    foreach (var child in dataSnap.Children)
                    {
                        string json = child.GetRawJsonValue();
                        DataUser userData = JsonUtility.FromJson<DataUser>(json);

                        if (string.IsNullOrEmpty(password))
                        {
                            if (userData.nameUser == user)
                            {
                                Debug.Log($"Tìm thấy user {userData.id} với tên {userData.nameUser}");
                                onComplete?.Invoke(true);
                                return;
                            }
                        }
                        else
                        {

                            if (userData.nameUser == user && userData.password == password)
                            {
                                Debug.Log($"Tìm thấy user {userData.id} với tên {userData.nameUser}");
                                onComplete?.Invoke(true);
                                return;
                            }
                        }
                    }
                }
                onComplete?.Invoke(false);


            }
            else
            {
                Debug.Log($"{task.Exception}");
                onComplete?.Invoke(false);
            }
        });
#endif
    }


    public IEnumerator PutDataCoroutine(DataUser user, Action<string> onCompele = null)
    {
        string path = "Users/" + user.id;
        string jsonData = JsonUtility.ToJson(user);
        string url = $"{dataURL}{path}.json";
        using (UnityWebRequest req = new UnityWebRequest(url, "PUT"))
        {
            byte[] body = Encoding.UTF8.GetBytes(jsonData);
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
                onCompele?.Invoke(req.downloadHandler.text);
            else Debug.Log("put error là" + req.error);

        }
    }

    public IEnumerator GetDataCoroutine(string name, string password = "", Action<string> onComplete = null)
    {
        string url = $"{dataURL}Users.json";
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {

                string toJson = ConvertFirebaseJsonToArray(req.downloadHandler.text);
                var user = JsonUtility.FromJson<UserList>(toJson);

                bool found = false;
                foreach (DataUser userFound in user.user)
                {
                    Debug.Log(userFound);

                    if (string.IsNullOrEmpty(password))
                    {

                        if (userFound.nameUser == name)
                        {
                            Debug.Log($"Tìm thấy {userFound}, id = {userFound.id}");
                            onComplete?.Invoke(userFound.nameUser);
                            found = true;
                            yield break;
                        }
                    }
                    else
                    {
                        if (userFound.nameUser == name && userFound.password == password)
                        {
                            Debug.Log($"Tìm thấy {userFound}, id = {userFound.id}");
                            onComplete?.Invoke(userFound.nameUser);
                            found = true;
                            yield break;
                        }
                    }
                }

                if (!found)
                {
                    onComplete?.Invoke(null);
                }
            }
            else
            {
                Debug.LogWarning("loi get error" + req.error);
                onComplete?.Invoke(null);
            }
        }
    }


    private string ConvertFirebaseJsonToArray(string json)
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
    private string EscapeJsonString(string s)
    {
        if (s == null) return "";
        return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

}
