using UnityEngine;

public class ExitBtn : ButtonBase
{
    public override void OnClick()
    {
        Time.timeScale = 1f;
        GameManager.Instance.IsPaused = false;
        UIManager.Instance.CurrentScene = UIManager.SceneType.GAMEPLAY;

        UIManager.Instance.ChangeScene(UIManager.SceneType.LOADING);
    }


}
