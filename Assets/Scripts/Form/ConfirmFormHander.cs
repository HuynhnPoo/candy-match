using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmFormHander : MonoBehaviour, ICompoment
{
    [SerializeField] private NameInput nameInput;
    [SerializeField] private PasswordInput passwordInput;
    [SerializeField] private ConfirmPassInput confirmPassInput;
    // [SerializeField] private Button jjjjj;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   public void ForgotPass()
    {
        string name = nameInput.nameID;
        string newPassword = passwordInput.password;
        string comfirmPass = confirmPassInput.comfirmPassword;

        CheckIsEmptyString(name,newPassword,comfirmPass);

        UserList users = new UserList().LoadUsers();

        DataUser founderUser = users.user.Find(u => u.nameUser == name);
        if (founderUser != null)
        {
            founderUser.password = newPassword;
            users.SaveData(users);
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
