#if !UNITY_WEBGL //  khong phải webgl thi sẽ udng duong thư vien này
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
#endif
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DatabaseFirebaseManager : SingletonBase<DatabaseFirebaseManager>
{
#if !UNITY_WEBGL
    private DatabaseReference dataRef; // biên firebase danh cho pc và mobie
#endif
    private string dataURL = "https://saga-candy-default-rtdb.asia-southeast1.firebasedatabase.app/"; // link api cho web

    public DataUser DataUserFound { get; set; } = null;

    private static DataUser userFound;
    public DataUser UserFound { set => userFound = value; get => userFound; }

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
            ReadDataOption(item.id, "", success =>
            {
                if (success)
                {
                    DataUser dataUser = DataUserFound;

                    if (dataUser != null && dataUser.nameUser == item.nameUser) Debug.LogWarning("ID này đã được tạo ");
                    else Debug.Log("cả ID và tên đã dduwojc sử dụng");

                }
                else WriteDataOption(item); //viêt các dữ liệu đọc được đưa lên firebase

            });
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

        DataUserFound = null;

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
                                DataUserFound = userData;
                                onComplete?.Invoke(true);
                                return;
                            }
                            if (userData.id == user)
                            {
                                Debug.Log($"Tìm thấy user {userData.id} với tên {userData.nameUser}");
                                DataUserFound = userData;
                                onComplete?.Invoke(true);
                                return;
                            }
                        }
                        else
                        {

                            if (userData.nameUser == user && userData.password == password)
                            {
                                Debug.Log($"Tìm thấy user {userData.id} với tên {userData.nameUser}");
                                DataUserFound = userData;
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
                //chuyen du lieuj sang json
                string toJson = GameMechanics.ConvertFirebaseJsonToArray(req.downloadHandler.text);
                var user = JsonUtility.FromJson<UserList>(toJson);

                bool found = false;
                foreach (DataUser userFound in user.user) // duyet qua tat car user
                {
                    if (string.IsNullOrEmpty(password))
                    {

                        if (userFound.nameUser == name)
                        {
                            DataUserFound = userFound;
                            onComplete?.Invoke(userFound.nameUser);
                            found = true;
                            yield break;
                        }
                        if (userFound.id == name)
                        {
                            DataUserFound = userFound;
                            onComplete?.Invoke(userFound.nameUser);
                            found = true;
                            yield break;
                        }
                    }
                    else
                    {
                        if (userFound.nameUser == name && userFound.password == password)
                        {
                            DataUserFound = userFound;
                            onComplete?.Invoke(userFound.nameUser);
                            found = true;
                            yield break;
                        }
                    }

                }

                if (!found)
                {
                    DataUserFound = null; ;
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

    //ham caapj nhap gias tri score va coin
    public void UpLoadCoinAndScore(int newCoin, int newScore)
    {

        if (UserFound == null)
        {
            Debug.LogError("UserFound là NULL! Không có current user.");
            return;
        }
        DataUser currentUser = UserFound;

        if (currentUser != null)
        {
            currentUser.z_coin = newCoin;
            currentUser.z_highScore = newScore;

            Debug.Log("hien thi ra user cập nhật " + currentUser.id + currentUser.z_highScore);
            if (GameMechanics.CheckInernet()) //ghi đè dữ liệu Coin và Score trên Firebase
                WriteDataOption(currentUser);
            else
            {
                // Tải danh sách người dùng hiện tại từ JSON
                UserList users = new UserList().LoadUsers();

                // Tìm index của người dùng hiện tại trong danh sách
                int index = users.user.FindIndex(u => u.id == currentUser.id);

                if (index != -1)
                {
                    // Ghi đè người dùng đã cập nhật
                    users.user[index] = currentUser;
                    users.SaveData(users);

                }
            }
        }

        else { Debug.LogWarning("chưa thấy current user để cập nhật "+(currentUser!=null?currentUser.id:"null")); }
    }

}
