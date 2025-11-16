public class BackMainBtn : ButtonBase
{
    public override void OnClick()
    {
        UIManager.Instance.ChangeScene(UIManager.SceneType.MAINMENU);
    }


}
