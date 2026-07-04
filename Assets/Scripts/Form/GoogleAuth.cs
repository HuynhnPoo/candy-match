using Firebase;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google;

public class GoogleAuth : MonoBehaviour
{
    [SerializeField] private string webAPI = "YOUR_WEB_CLIENT_ID_HERE.apps.googleusercontent.com";
    private FirebaseAuth firebaseAuth;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                firebaseAuth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.Log($"không thể khởi tạo {dependencyStatus}");
            }
        });
    }

    public void OnGoogleLoginClick()
    {
        Google.GoogleSignInConfiguration configuration = new Google.GoogleSignInConfiguration
        {
            WebClientId = webAPI,
            RequestIdToken = true
        };

        Google.GoogleSignIn.Configuration = configuration;
        Google.GoogleSignIn.Configuration.UseGameSignIn = false;
        Google.GoogleSignIn.Configuration.RequestIdToken = true;

        Google.GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Lỗi đăng nhập Google.");
            }
            else if (task.IsCanceled)
            {
                Debug.Log("Người dùng hủy đăng nhập.");
            }
            else
            {
                string idToken = task.Result.IdToken;
                SignInWithFirebase(idToken);
            }
        });
    }

    private void SignInWithFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        firebaseAuth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("Firebase Sign In bị hủy.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Firebase Sign In gặp lỗi: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;
            Debug.LogFormat("Chào mừng {0} đã đăng nhập thành công vào Firebase!", newUser.DisplayName);
        });
    }

    void Update()
    {
        // Để trống hoặc xóa đi nếu không dùng
    }
}