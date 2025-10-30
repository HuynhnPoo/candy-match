using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    private static bool isPaused = false;
    public bool IsPaused { get => isPaused; set => isPaused = value; }

    private int score = 0;
    private static int highScore = 0;
    public int Score { get => score; set => score = value; }

    public string NameUserLogin { set; get; } = "";

    public string Notification { get; set; } = "null";

    private  static int moveStep = 25;
    public int MoveStep { set => moveStep = value; get => moveStep; }

 
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
