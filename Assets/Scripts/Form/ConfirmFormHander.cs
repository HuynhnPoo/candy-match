using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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
        UserList users = new UserList().LoadUsers();
        string nameUser = nameInput.nameID;
        string newPassword = passwordInput.password;
        string comfirmPass = confirmPassInput.comfirmPassword;

        if (!CheckIsEmptyString(nameUser, newPassword, comfirmPass)) return;

        if (GameMechanics.CheckInernet())
        {

            DatabaseFirebaseManager.Instance.ReadDataOption(nameUser,"", (success) =>
            {

                Debug.Log(success);
                if (success)
                {
                    string newId = DatabaseFirebaseManager.Instance.DataUserFound.id;

                    Debug.Log("hien thi" + newId);


                    UIManager.Instance.ShowNotification(false, "Thực hiện thành công quên mật khảu"); // dang kí up len firebase
                    DatabaseFirebaseManager.Instance.WriteDataOption(new DataUser { id = newId, nameUser = nameUser, password = newPassword });



                }
                else
                {
                    UIManager.Instance.ShowNotification(false, "Thực hiện không thành công quên mật khẩu");
                    return;
                }
            });
        }
        else
        {
            DataUser founderUser = users.user.Find(u => u.nameUser == nameUser);
            if (founderUser == null)
            {
                UIManager.Instance.ShowNotification(false, "Thực hiện không thành công quên mật khẩu");
                return;
            }
            else
            {

                founderUser.password = newPassword;
                string idUser = founderUser.id;
                UIManager.Instance.ShowNotification(false, "Thực hiện thành công quên mật khẩu");

                users.SaveData(users);
                DatabaseFirebaseManager.Instance.WriteDataOption(new DataUser { id = idUser, nameUser = name, password = newPassword });
            }

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
