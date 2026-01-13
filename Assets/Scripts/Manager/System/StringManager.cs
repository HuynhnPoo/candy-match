
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public static class StringManager
{
    public static readonly string pathDataUser = Path.Combine(Application.persistentDataPath, "users.json");

    public static readonly string firebaseUser = "Users";
    public static readonly string saveBuyItem = "SaveBuyItem";

    // playpref coin score



    // scene form

    public static readonly string gameCTRTag = "GameController";
    public static readonly string forgotCanvas = "Forgot_Canvas";
    public static readonly string LoginCanvas = "Login_Canvas";


    //scene gameplay

    public static readonly string gameOverPn = "GameResult_Panel";
    public static readonly string pausePn = "Pause_Panel";

    // save playfrep

    public static readonly string coinSaveStr = "CoinSave";
    public static readonly string highScoreStr = "HighScore";
    
    public static readonly string musicSave = "MusicSave";
    public static readonly string sfxSave = "SFXSave";



    // boost game

    public static readonly string[] itemBoosts =
    {
        "BoostTurn",
        "BoostTime",
        "BoostGold"
    };

   

}
