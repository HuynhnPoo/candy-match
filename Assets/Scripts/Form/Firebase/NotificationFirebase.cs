using Firebase;
using Firebase.Extensions;
using Firebase.Messaging;
using System;
using UnityEngine;

public class NotificationFirebase : MonoBehaviour
{
    private bool isInitialized = false;

    private void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
     //   InitializeFirebase();

          InitNotification();
#else
        Debug.Log("Editor Mode: Firebase Messaging không được khởi tạo.");

        // Test giả lập sau 3 giây
    //    Invoke(nameof(TestNotificationInEditor), 3f);
#endif
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                DependencyStatus dependencyStatus = task.Result;

                if (dependencyStatus == DependencyStatus.Available)
                {
                    InitNotification();
                }
                else
                {
                    Debug.LogError($"Không thể thiết lập Firebase: {dependencyStatus}");
                }
            });
    }

    void InitNotification()
    {
        if (isInitialized)
            return;

        isInitialized = true;

        FirebaseMessaging.MessageReceived += OnMessageReceived;
        FirebaseMessaging.TokenReceived += OnTokenReceived;

        Debug.Log("Firebase Messaging khởi tạo thành công!");
    }

    private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        Debug.Log("FCM Token: " + e.Token);
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        if (e.Message.Notification != null)
        {
            string title = e.Message.Notification.Title;
            string body = e.Message.Notification.Body;

            Debug.Log($"Thông báo nhận được: {title} - {body}");

            // Hiển thị UI ở đây nếu cần
            // notificationUI.Show(title, body);
        }

        if (e.Message.Data != null && e.Message.Data.Count > 0)
        {
            foreach (var data in e.Message.Data)
            {
                Debug.Log($"Key: {data.Key} | Value: {data.Value}");
            }
        }
    }

#if UNITY_EDITOR
    // Dùng để test trên Editor
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestNotificationInEditor();
        }
    }

    void TestNotificationInEditor()
    {
        Debug.Log("=== THÔNG BÁO GIẢ LẬP ===");

        string title = "Test Notification";
        string body = "Đây là thông báo giả lập từ Unity Editor.";

        Debug.Log($"Title: {title}");
        Debug.Log($"Body: {body}");

        // Nếu có UI:
        // notificationUI.Show(title, body);
    }
#endif

    private void OnDestroy()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (isInitialized)
        {
            FirebaseMessaging.MessageReceived -= OnMessageReceived;
            FirebaseMessaging.TokenReceived -= OnTokenReceived;
        }
#endif
    }
}