using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UIManager;

public class GameManager : SingletonBase<GameManager>
{
    private static bool isPaused = false;
    public bool IsPaused { get => isPaused; set => isPaused = value; }

    public bool IsGameOver { set; get; } = false;
    public bool IsWinGame { set; get; } = false; 
    public bool IsClearCandy { set; get; } = false;
    public bool IsChangeCandy { set; get; } = false;

    bool hasEndGame = false;
    private int score = 0;
    public int HighScore { set; get; } = 0;

    public int Score { get => score; set => score = value; }

    public int Coin { set; get; } = 0; 
    public int TotalCoin { set; get; } = 0;
    public int CoinDown { set; get; } = 0;
    public int ScoreDown { set; get; } = 1;
    public int increaseGoldBoost { set; get; } = 1;

    private static int moveStep=25;
    public int MoveStep { set => moveStep = value; get => moveStep; }

    private static int currentLevel;
    public int CurrentLevel { set => currentLevel = value; get => currentLevel; }
    public string NameUserLogin { set; get; } = "";
    public float time { set; get; } = 30;

    public string Notification { get; set; } = "null";
    public string StatusGameStr { get; set; } = "null";

    public event Action OnGameOver;
    public Action<string> OnboostGame;

    [SerializeField] private GridManager gridManager;

    private void OnEnable()
    {
        //Coin = 10000;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadScene)
    {
        if (scene.name != "BOOTSTRAP")
        {
            if (SceneManager.GetActiveScene().name == SceneType.GAMEPLAY.ToString())
            {
                this.gridManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridManager>();

            }
          else this.gridManager = null;
        }

    }


    public void Init()
    {
        score = 0;
        hasEndGame = false;
        IsWinGame = false;
        IsGameOver = false;

        Debug.Log("hien thi ra"+time +" "+increaseGoldBoost+" "+moveStep);
        GameMechanics.Init(time, increaseGoldBoost);
        OnGameOver = GameOver;

    }


    public void Pausing(bool paused)
    {
        if (!paused)  // kiểm tra xem nêu chưa pause thi thực hiện pause
        {
            Time.timeScale = 0f;
            Debug.Log(UIManager.Instance.pausePn);
            UIManager.Instance.pausePn.SetActive(true);
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            UIManager.Instance.pausePn.SetActive(false);
            isPaused = false;
        }
    }

    private void Update()
    {
       /* if ((IsWinGame || IsGameOver) && !hasEndGame)
        {
            hasEndGame = true;
            Debug.Log("thuc hien end game");
            OnGameOver?.Invoke();

        }*/
    }

    public void GameOver()
    {
        UIManager.Instance.gameoverPn.SetActive(true);
      
        if (IsGameOver)
        {
            StatusGameStr = "Game Over";
        }
        else if (IsWinGame)
        {
            StatusGameStr = "Win Game";
            UIManager.Instance.gameoverPn.transform.GetChild(2).gameObject.SetActive(true);
        }

        GameMechanics.CalculateMoney(score, (int)GameMechanics.CountDown(), moveStep);
        GameMechanics.CheckHightScore(score);
      //  DatabaseFirebaseManager.Instance.UpLoadCoinAndScore(Coin,HighScore);

        Time.timeScale = 0f;

    }

    public void ShuffleCandyBoost()
    {
        StartCoroutine(ShuffleCandy.ShuffleBoard(gridManager,gridManager.visualGrid));
    }

}
