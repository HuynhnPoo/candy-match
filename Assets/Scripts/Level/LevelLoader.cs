using UnityEngine;

public class LevelLoader : ButtonBase
{
    [SerializeField] private int currentLevel;

    public override void OnClick()
    {
        GameManager.Instance.CurrentLevel = currentLevel;

        UIManager.Instance.CurrentScene = UIManager.SceneType.LEVELMENU;
        UIManager.Instance.ChangeScene(UIManager.SceneType.LOADING);
    }


}
