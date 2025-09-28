using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class FormHander : MonoBehaviour, ICompoment
{
    [SerializeField] private NameInput nameInput;
    [SerializeField] private PasswordInput passwordInput;
    [SerializeField] private ConfirmPassInput confirmPassInput;
    string path;

    public void LoadCompoment()
    {
        nameInput = GetComponentInChildren<NameInput>();
        passwordInput = GetComponentInChildren<PasswordInput>();
    }
    private void Awake()
    {
        LoadCompoment();

    }

    public void Register()
    {

        UserList users = new UserList().LoadUsers();

        string nameUser = nameInput.nameID;
        string password = passwordInput.password;


        if (!CheckIsEmptyString(nameUser, password)) return;

        if (users.user.Exists(u => u.nameUser == nameUser))
        {
            UIManager.Instance.ShowNotification(false, "Thực hiện không thành công đăng kí");

            GameManager.Instance.Notification = "Thực hiện không thành công đăng kí";
            return;
        }
        else
        {
            string newId = users.GetIDAccout();
            users.user.Add(new DataUser { id = newId, nameUser = nameUser, password = password });

            Debug.Log("hien thi ra " + newId + " " + nameUser + "và ");

            users.SaveData(users);
            UIManager.Instance.ShowNotification(false, "Thực hiện thành công đăng kí");

            GameManager.Instance.Notification = "Thực hiện thành công đăng kí";
            DatabaseFirebaseManager.Instance.WriteDataOption(new DataUser { id = newId, nameUser = nameUser, password = password });

        }
    }

    public void Login()
    {
        UserList users = new UserList().LoadUsers();
        string nameUser = nameInput.nameID;
        string password = passwordInput.password;

        if (!CheckIsEmptyString(nameUser, password)) return;


        if (CheckInernet())
        {
            DatabaseFirebaseManager.Instance.ReadDataOption(nameUser, (success) =>
            {
                if (success)
                {
                    UIManager.Instance.ShowNotification(true, "Thực hiện thành công đăng nhập");
                }
                else
                {
                    UIManager.Instance.ShowNotification(false, "Thực hiện không thành công đăng nhập");
                }
            });
        }
        else
        {
            DataUser foundUser = users.user.Find(u => u.nameUser == nameUser && u.password == password);

            if (foundUser != null)
            {
                UIManager.Instance.ShowNotification(true, "Thực hiện thành công đăng nhập offline");
       
            }
            else
            {
                UIManager.Instance.ShowNotification(false, "Thực hiện thành công đăng nhập offline");
            }
        }
    }

    bool CheckInernet()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }



    bool CheckIsEmptyString(string name, string pass)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
        {
            return false;
        }
        return true;
    }

}
