using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMechanics
{
    private static int score = 0;

    private static float time = 30;
    private static bool timeUp = false;

    public static void Init()
    {
        score = 0;
        time = 30;
        timeUp = false;
    }
    
    public static int AddScore(int amout)
    {
        score += amout; // thêm điểm
        Debug.Log("diemrd cua game là " + score);
        return score;
    }


    public static float CountDown()
    {
        if (!timeUp)
        {
            time -= Time.deltaTime;
         //   Debug.Log("hien thi ra "+ time);
            if (time <= 0)
            {
                timeUp = true;
                time = 0;

                Debug.Log("het gio");

            }
        }
        return time;
    }
}