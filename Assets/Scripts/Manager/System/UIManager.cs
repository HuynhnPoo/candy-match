using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingletonBase<UIManager>
{
    [SerializeField] public GameObject loginForm { get; private set; }
    public GameObject forgotForm { get; private set; }
    public GameObject managerCanvas { get; private set; }



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
            managerCanvas = GameObject.FindGameObjectWithTag(StringManager.gameCTRTag);
            loginForm = FindGameObjectByNameHide.FindGameObjectByName(StringManager.LoginCanvas);
            forgotForm = FindGameObjectByNameHide.FindGameObjectByName(StringManager.forgotCanvas);

        }
    }
    public enum SceneType
    {
        FORM = 0,
        MAINMENU,
        GAMEPLAY,
        LOADING
    }

    public AsyncOperation ChangeScene(SceneType scene)
    {
        return SceneManager.LoadSceneAsync(scene.ToString());
    }
}
