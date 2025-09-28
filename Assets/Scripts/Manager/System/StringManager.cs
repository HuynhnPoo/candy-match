
using System.IO;
using UnityEngine;
public static class StringManager
{
    public static readonly string pathDataUser = Path.Combine(Application.persistentDataPath, "users.json") ;

    public static readonly string firebaseUser = "Users";

    // scene form

    public static readonly string gameCTRTag = "GameController";
    public static readonly string forgotCanvas = "Forgot_Canvas";
    public static readonly string LoginCanvas = "Login_Canvas";


    //scene gameplay

    public static readonly string gameOverPn= "GameOver_Panel";
    public static readonly string pausePn= "Pause_Panel";
}
