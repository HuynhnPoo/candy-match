using System.Collections;
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
    [SerializeField] private GameObject canvasFade;


    // game obj main menu
    public GameObject shopCanvas { get; private set; }
    public int QuatityItem { set; get; }

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
  //  private float lastTransitionTime = 0f;
    private const float transitionCooldown = 1.0f; // Ngăn chặn spam

    protected void Update()
    {

     /*   if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(GameManager.Instance.CoinDown + " " + GameManager.Instance.ScoreDown);
        }*/
    }

    private void Init()
    {
        if (SceneManager.GetActiveScene().name == SceneType.FORM.ToString())
        {
            this.managerCanvas = GameObject.FindGameObjectWithTag(StringManager.gameCTRTag);
            this.loginForm = FindGameObjectByNameHide.FindGameObjectByName(StringManager.LoginCanvas);
            this.forgotForm = FindGameObjectByNameHide.FindGameObjectByName(StringManager.forgotCanvas);

            this.notificationMess = FindGameObjectByNameHide.FindGameObjectByName("Backgroud-noti");

            this.canvasFade = FindGameObjectByNameHide.FindGameObjectByName("Canvas_AniFade");

        }
        else if (SceneManager.GetActiveScene().name == SceneType.GAMEPLAY.ToString())
        {
            this.gameoverPn = FindGameObjectByNameHide.FindGameObjectByName(StringManager.gameOverPn);
            this.pausePn = FindGameObjectByNameHide.FindGameObjectByName(StringManager.pausePn);

        }
        else if (SceneManager.GetActiveScene().name == SceneType.MAINMENU.ToString())
        {
            this.shopCanvas = FindGameObjectByNameHide.FindGameObjectByName("Shop_Canvas");
        }
    }

    public enum SceneType
    {
        FORM = 0,
        MAINMENU,
        LOADING,
        LEVELMENU,
        GAMEPLAY
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

        if (isLogin)
        {
            if (canvasFade == null)
            {
                Debug.LogError("Không tìm thấy Canvas_AniFade! Bỏ qua hiệu ứng và chuyển thẳng Scene.");
                ChangeScene(SceneType.MAINMENU);
                yield break;
            }
            else
            {

            canvasFade.SetActive(true);
            AnimationFade fade = canvasFade.GetComponent<AnimationFade>();
            fade.PlayAni("End_Trig");
            yield return new WaitForSeconds(0.6f);

            AsyncOperation loadOperation = ChangeScene(SceneType.MAINMENU);
            loadOperation.allowSceneActivation = false;
            while (loadOperation.progress < 0.9f)
            {
                yield return null;
            }

            loadOperation.allowSceneActivation = true;
            yield return loadOperation;

            fade.PlayAni("Start_Trig");
            yield return new WaitForSeconds(0.7f);

            canvasFade.SetActive(false);
            }
        }
    }


    public void ShowNotificationBuy()
    {
        StartCoroutine(ShowNotificationCorutine());
    }

    IEnumerator ShowNotificationCorutine() 
    {
        shopCanvas.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        shopCanvas.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
    }
    

    public AsyncOperation ChangeScene(SceneType scene)
    {
        return SceneManager.LoadSceneAsync(scene.ToString());
    }
}
