using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FormHander : MonoBehaviour,ICompoment
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

        if (users.user.Exists(u => u.nameUser == nameUser)) return;

        users.user.Add(new DataUser { nameUser = nameUser, password = password });

       users.SaveData(users);

        Debug.Log("dang ki thanh con tai khoan" + nameUser);
    }

    public void Login()
    {
        UserList users = new UserList().LoadUsers();
        string nameUser = nameInput.nameID;
        string password = passwordInput.password;


        if (!CheckIsEmptyString(nameUser, password)) return;


        DataUser foundUser = users.user.Find(u => u.nameUser == nameUser && u.password == password);

        if (foundUser != null)
        {
            Debug.Log("thuc hien thanh cong dang nhap");
        }
        else
        {
            Debug.Log("thuc hien dang nhap khong thanh cong");
        }

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
