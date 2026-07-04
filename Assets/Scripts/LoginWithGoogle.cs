using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UI = UnityEngine.UI; // Tránh trùng tên Image với Firebase hoặc các thư viện khác
using UnityEngine.Networking;
using UnityEngine.SceneManagement; // Thêm thư viện để chuyển Scene
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;

public class LoginWithGoogle : MonoBehaviour
{
    [Header("Google API")]


    private string googleAPI = "945438211481-7cb5pstsj910fmj1ub4u62pilqm3e0qo.apps.googleusercontent.com";
    private bool isGoogleSignInInitialized = false;

    [Header("Firebase Auth")]
    private FirebaseAuth auth;
    private FirebaseUser user;

    //[Header("UI References (Optional)")]
    //public TMP_Text usernameText;
    //public TMP_Text userEmailText;
    //public UI.Image userProfilePic;

    //[Header("Scene Configuration")]
    //[SerializeField] private string gameplaySceneName = "GameplayScene"; // Tên Scene muốn chuyển đến

    private void Start()
    {
        InitFirebase();
    }

    private void InitFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Login()
    {
        Debug.Log("============== [LOG FLOW] STEP 1: Click nút Login ==============");

        // Đảm bảo auth không bị null trước khi xử lý
        if (auth == null)
        {
            auth = FirebaseAuth.DefaultInstance;
            Debug.Log("[LOG FLOW] Firebase Auth được khởi tạo lại vì bị null.");
        }

        // --- CHẠY TRÊN THIẾT BỊ ANDROID THẬT ---
        if (!isGoogleSignInInitialized)
        {
            Debug.Log("[LOG FLOW] STEP 1.1: Khởi tạo cấu hình GoogleSignInConfiguration...");
            Debug.Log($"[LOG FLOW] Sử dụng WebClientId: {googleAPI}");

            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                WebClientId = googleAPI,
                RequestEmail = true
            };
            isGoogleSignInInitialized = true;
        }

        Debug.Log("[LOG FLOW] STEP 2: Bắt đầu gọi hộp thoại Đăng nhập Google (GoogleSignIn.SignIn)...");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("[LOG FLOW] [THẤT BẠI] Người dùng đã hủy (Cancel) hộp thoại đăng nhập Google.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("[LOG FLOW] [THẤT BẠI] Lỗi kết nối Google Sign-In: " + task.Exception);
                return;
            }

            Debug.Log("[LOG FLOW] STEP 3: Đăng nhập Google THÀNH CÔNG! Đang lấy thông tin User...");
            GoogleSignInUser googleUser = task.Result;

            Debug.Log($"[LOG FLOW] Google User Name: {googleUser.DisplayName}");
            Debug.Log($"[LOG FLOW] Google User Email: {googleUser.Email}");
            Debug.Log($"[LOG FLOW] Google IdToken (Rút gọn): {googleUser.IdToken.Substring(0, Mathf.Min(10, googleUser.IdToken.Length))}...");

            Debug.Log("[LOG FLOW] STEP 4: Tạo Credential từ IdToken để gửi sang Firebase...");
            Credential credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);

            Debug.Log("[LOG FLOW] STEP 5: Đang gửi Credential sang Firebase Auth để xác thực...");
            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(authTask =>
            {
                if (authTask.IsCanceled)
                {
                    Debug.LogWarning("[LOG FLOW] [THẤT BẠI] Tiến trình xác thực Firebase bị hủy.");
                    return;
                }
                if (authTask.IsFaulted)
                {
                    Debug.LogError("[LOG FLOW] [THẤT BẠI] Xác thực Firebase THẤT BẠI: " + authTask.Exception);
                    return;
                }

                Debug.Log("[LOG FLOW] STEP 6: Xác thực Firebase THÀNH CÔNG!");
                user = auth.CurrentUser;

                Debug.Log($"[LOG FLOW] Firebase User ID: {user.UserId}");
                Debug.Log($"[LOG FLOW] Firebase Email: {user.Email}");

                // Bạn đang tạm thời bỏ qua Database để chuyển thẳng vào game:
                Debug.Log("[LOG FLOW] STEP 7: (Tạm bỏ qua HandleUserDatabase) - Chuẩn bị gọi ProceedToGame()...");

                // Sau khi Firebase Auth thành công, xử lý check data dưới Database
                 HandleUserDatabase(user);

               // ProceedToGame();
            });
        });
    }

    /// <summary>
    /// Hàm kiểm tra và xử lý dữ liệu User dưới Database
    /// </summary>
    private void HandleUserDatabase(FirebaseUser firebaseUser)
    {
        string userId = "google_" + firebaseUser.UserId;
        string defaultName = string.IsNullOrEmpty(firebaseUser.DisplayName) ? "Player_" + Random.Range(1000, 9999) : firebaseUser.DisplayName;

        // Gọi sang Manager quản lý Firebase Database của bạn để kiểm tra dữ liệu
        // Ở đây mình giả định bạn dùng một hàm tương tự như các project trước của bạn:
        DatabaseFirebaseManager.Instance.ReadDataOption(userId, "", success =>
        {
            DataUser finalUser;

            if (success)
            {
                // TH 1: ĐÃ CÓ TÀI KHOẢN -> Đọc dữ liệu cũ lên
                finalUser = DatabaseFirebaseManager.Instance.DataUserFound;
                Debug.Log($"[Google Login] Tài khoản cũ: {finalUser.nameUser} | Coins: {finalUser.z_coin}");
            }
            else
            {
                // TH 2: CHƯA CÓ TÀI KHOẢN -> Tự động tạo mới (Auto Register)
                finalUser = new DataUser
                {
                    id = userId,
                    nameUser = defaultName,
                    password = "", // Không cần pass vì login qua Google
                    z_coin = 0,    // Giá trị khởi tạo mặc định
                    z_highScore = 0
                };

                // Lưu tài khoản mới lên Firebase Database
                DatabaseFirebaseManager.Instance.WriteDataOption(finalUser);
                Debug.Log($"[Google Login] Đã tự động đăng ký tài khoản mới: {finalUser.nameUser}");
            }

            // Đồng bộ dữ liệu vào hệ thống toàn cục của Game
            DatabaseFirebaseManager.Instance.DataUserFound = finalUser;
            DatabaseFirebaseManager.Instance.UserFound = finalUser;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.NameUserLogin = finalUser.nameUser;
                GameManager.Instance.CoinDown = finalUser.z_coin;
                GameManager.Instance.ScoreDown = finalUser.z_highScore;
            }

            // Tiến hành chuyển Scene sau khi mọi thứ đã sẵn sàng
            ProceedToGame();
        });
    }

    private void ProceedToGame()
    {
        Debug.Log("Mọi dữ liệu đã đồng bộ. Đang chuyển sang Scene tiếp theo...");
        SceneManager.LoadScene(UIManager.SceneType.MAINMENU.ToString());
    }

#if UNITY_EDITOR
    private IEnumerator SimulateEditorLogin()
    {
        yield return new WaitForSeconds(1f); // Giả lập delay mạng 1 giây

        string mockId = "google_editor_test_888";

        DatabaseFirebaseManager.Instance.ReadDataOption(mockId, "", success =>
        {
            DataUser finalUser;
            if (success)
            {
                finalUser = DatabaseFirebaseManager.Instance.DataUserFound;
            }
            else
            {
                finalUser = new DataUser
                {
                    id = mockId,
                    nameUser = "Editor_Player_" + Random.Range(1000, 9999),
                    password = "",
                    z_coin = 100,
                    z_highScore = 10
                };
                DatabaseFirebaseManager.Instance.WriteDataOption(finalUser);
            }

            DatabaseFirebaseManager.Instance.DataUserFound = finalUser;
            DatabaseFirebaseManager.Instance.UserFound = finalUser;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.NameUserLogin = finalUser.nameUser;
                GameManager.Instance.CoinDown = finalUser.z_coin;
                GameManager.Instance.ScoreDown = finalUser.z_highScore;
            }

            ProceedToGame();
        });
    }
#endif

    public void SignOut()
    {
        if (auth != null)
        {
            auth.SignOut();
        }
        GoogleSignIn.DefaultInstance.SignOut();
        Debug.Log("Đã đăng xuất tài khoản Google và Firebase.");
    }
}