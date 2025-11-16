public class PlayGameBtn : ButtonBase
{
    public override void OnClick()
    {
        UIManager.Instance.ChangeScene(UIManager.SceneType.LEVELMENU);
    }

}
