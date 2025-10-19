using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameManager : SingletonBase<GameManager>
{
    private static bool isPaused = false;
    public bool IsPaused { get => isPaused; set => isPaused = value; }

    private int score = 0;
    private static int highScore = 0;
    public int Score { get => score; set => score = value; }

    public string Notification { get; set; } = "fsfs";

    public void Init()
    {
        GameMechanics.Init();
        score = 0;
    }

    public void Pausing(bool paused)
    {
        if (!paused)  // kiểm tra xem nêu chưa pause thi thực hiện pause
        {
            Time.timeScale = 0f;
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


    public void SaveScore()
    {

    }

}
