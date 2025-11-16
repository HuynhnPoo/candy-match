public class LogOutBtn : ButtonBase
{
    public override void OnClick()
    {
        UIManager.Instance.ChangeScene(UIManager.SceneType.FORM); // hien thi ten dang nhap
    }

}
