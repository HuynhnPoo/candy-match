public class LogOutBtn : ButtonBase
{
    public override void OnClick()
    {
        DatabaseFirebaseManager.Instance.DataUserFound =null;
        UIManager.Instance.ChangeScene(UIManager.SceneType.FORM); // hien thi ten dang nhap
    }

}
