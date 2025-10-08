using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmFormHander : MonoBehaviour, ICompoment
{
    [SerializeField] private NameInput nameInput;
    [SerializeField] private PasswordInput passwordInput;
    [SerializeField] private ConfirmPassInput confirmPassInput;
   

    string path;

    public void LoadCompoment()
    {
        nameInput = GetComponentInChildren<NameInput>();
        passwordInput = GetComponentInChildren<PasswordInput>();
        confirmPassInput = GetComponentInChildren<ConfirmPassInput>();
    }
    private void Awake()
    {
        LoadCompoment();
    }

    public void ForgotPass()
    {
        string name = nameInput.nameID;
        string newPassword = passwordInput.password;
        string comfirmPass = confirmPassInput.comfirmPassword;

        CheckIsEmptyString(name, newPassword, comfirmPass);

        UserList users = new UserList().LoadUsers();

        DataUser founderUser = users.user.Find(u => u.nameUser == name);
        if (founderUser == null)
        {
            UIManager.Instance.ShowNotification(false, "Thực hiện không thành công đăng kí");
            return;
        }
        else
        {

            founderUser.password = newPassword;
            string idUser = founderUser.id;
            UIManager.Instance.ShowNotification(false, "Thực hiện thành công đăng kí");

            users.SaveData(users);
            DatabaseFirebaseManager.Instance.WriteDataOption(new DataUser { id = idUser, nameUser = name, password = newPassword });
        }
    }


    bool CheckIsEmptyString(string name, string pass, string confirmPass)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(confirmPass))
            return false;

        if (pass != confirmPass) return false;
        return true;
    }
}
