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
        if (GameMechanics.CheckInernet()) // kiem tra có internet có mạng để dang kí đây lên firebase
        {
            DatabaseFirebaseManager.Instance.ReadDataOption(nameUser, "", (success) =>
            {
                if (success)
                {
                    UIManager.Instance.ShowNotification(false, "Thực hiện không thành công đăng kí,tài khoản đã tồn tại");
                }
                else
                {
                    string newId = users.GetIDAccout();
                    UIManager.Instance.ShowNotification(false, "Thực hiện thành công đăng kí"); // dang kí up len firebase
                    DatabaseFirebaseManager.Instance.WriteDataOption(new DataUser { id = newId, nameUser = nameUser, password = password });

                    //dang kí ghi vào json
                    users.user.Add(new DataUser { id = newId, nameUser = nameUser, password = password, z_coin = 100, z_highScore = 0 });

                    users.SaveData(users);
                }
            });
        }
        else
        {

            if (users.user.Exists(u => u.nameUser == nameUser))
            {
                UIManager.Instance.ShowNotification(false, "Thực hiện không thành công đăng kí");

                return;
            }
            else
            {
                string newId = users.GetIDAccout();
                users.user.Add(new DataUser { id = newId, nameUser = nameUser, password = password, z_coin = 100, z_highScore = 0 });

                Debug.Log("hien thi ra " + newId + " " + nameUser + "và ");

                users.SaveData(users);
                UIManager.Instance.ShowNotification(false, "Thực hiện thành công đăng kí");
                DatabaseFirebaseManager.Instance.WriteDataOption(new DataUser { id = newId, nameUser = nameUser, password = password });

            }
        }

    }

    public void Login()
    {
        UserList users = new UserList().LoadUsers();
        string nameUser = nameInput.nameID;
        string password = passwordInput.password;

        if (!CheckIsEmptyString(nameUser, password)) return;


        if (GameMechanics.CheckInernet())
        {
            DatabaseFirebaseManager.Instance.ReadDataOption(nameUser, password, (success) =>
            {


                if (success)
                {
                    DataUser currentUser = DatabaseFirebaseManager.Instance.DataUserFound;
                    DatabaseFirebaseManager.Instance.UserFound = currentUser;
                    GameManager.Instance.NameUserLogin = currentUser.nameUser; // gawn ten nhân vật cho NameUserLogin để quan lí
                    GameManager.Instance.CoinDown = currentUser.z_coin;
                    GameManager.Instance.ScoreDown = currentUser.z_highScore;

                    Debug.Log("hien thi "+currentUser.nameUser);
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
                DatabaseFirebaseManager.Instance.DataUserFound = foundUser;
                GameManager.Instance.NameUserLogin = foundUser.nameUser; // gawn ten nhân vật cho NameUserLogin để quan lí
                GameManager.Instance.CoinDown = foundUser.z_coin;
                GameManager.Instance.ScoreDown = foundUser.z_highScore;

                UIManager.Instance.ShowNotification(true, "Thực hiện thành công đăng nhập offline");
            }
            else
            {
                UIManager.Instance.ShowNotification(false, "Thực hiện không thành công đăng nhập offline");
            }
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
