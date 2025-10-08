using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DataUser
{
    public string id;
    public string nameUser;
    public string password;
}

[System.Serializable]
public class UserList
{
    private static int index = 0;
    public List<DataUser> user = new List<DataUser>();

    public UserList LoadUsers()
    {
        if (File.Exists(StringManager.pathDataUser))
        {
            string json = File.ReadAllText(StringManager.pathDataUser);
            return JsonUtility.FromJson<UserList>(json);
        }
        else
        {
            return new UserList();
        }
    }

    public void SaveData(UserList user)
    {
        string json = JsonUtility.ToJson(user, true);
        File.WriteAllText(StringManager.pathDataUser, json);
    }

    public string GetIDAccout()
    {
        UserList user = LoadUsers();
        string id;
        do
        {
            index++;
            id = "NV" + index.ToString("D2");
        } while (user.user.Exists(user => user.id == id));
        return id;
    }
    
}

