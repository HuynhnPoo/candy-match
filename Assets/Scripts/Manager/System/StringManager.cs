
using System.IO;
using UnityEngine;
public static class StringManager
{
    public static readonly string pathDataUser = Path.Combine(Application.persistentDataPath, "users.json");

    public static readonly string firebaseUser = "Users";
    public static readonly string saveBuyItem = "SaveBuyItem";


    // scene form

    public static readonly string gameCTRTag = "GameController";
    public static readonly string forgotCanvas = "Forgot_Canvas";
    public static readonly string LoginCanvas = "Login_Canvas";


    //scene gameplay

    public static readonly string gameOverPn = "GameOver_Panel";
    public static readonly string pausePn = "Pause_Panel";

    // save playfrep

    public static readonly string highScoreStr = "HighScore";

    // boost game

    public static readonly string increaseTurn = "BoostTurn";
    public static readonly string increaseTime = "BoostTime";


}
