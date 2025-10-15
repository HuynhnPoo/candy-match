using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingletonBase<UIManager>
{
    public SceneType CurrentScene { get; set; } = SceneType.FORM;

    // game obj scene form
    [SerializeField] public GameObject loginForm { get; private set; }
    public GameObject forgotForm { get; private set; }
    public GameObject notificationMess { get; private set; }
    public GameObject managerCanvas { get; private set; }

    // game obj scene gameplay

    public GameObject pausePn { get; private set; }
    public GameObject gameoverPn { get; private set; }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name != "BOOTSTRAP")
        {
            Init();
        }
    }


    private void Init()
    {
        if (SceneManager.GetActiveScene().name == SceneType.FORM.ToString())
        {
            this.managerCanvas = GameObject.FindGameObjectWithTag(StringManager.gameCTRTag);
            this.loginForm = FindGameObjectByNameHide.FindGameObjectByName(StringManager.LoginCanvas);
            this.forgotForm = FindGameObjectByNameHide.FindGameObjectByName(StringManager.forgotCanvas);
            this.notificationMess = FindGameObjectByNameHide.FindGameObjectByName("Backgroud-noti");
        }
        else if (SceneManager.GetActiveScene().name == SceneType.GAMEPLAY.ToString())
        {
            this.gameoverPn = FindGameObjectByNameHide.FindGameObjectByName(StringManager.gameOverPn);
            this.pausePn = FindGameObjectByNameHide.FindGameObjectByName(StringManager.pausePn);
        }
    }
    public enum SceneType
    {
        FORM = 0,
        MAINMENU,
        GAMEPLAY,
        LOADING
    }

    public void ShowNotification(bool isLogin, string notification)
    {
        notificationMess.SetActive(true);
        GameManager.Instance.Notification = notification;
        StartCoroutine(HideNofication(isLogin));
    }

    IEnumerator HideNofication(bool isLogin)
    {
        yield return new WaitForSeconds(0.8f);
        notificationMess?.SetActive(false);
        yield return null;

        if (isLogin) UIManager.Instance.ChangeScene(UIManager.SceneType.MAINMENU);
    }

    public AsyncOperation ChangeScene(SceneType scene)
    {
        return SceneManager.LoadSceneAsync(scene.ToString());
    }
}
