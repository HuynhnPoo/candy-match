public class ConfirmBtn : ButtonBase
{
    private ConfirmFormHander hander;
    public override void OnClick()
    {
        hander = UIManager.Instance.managerCanvas.GetComponentInChildren<ConfirmFormHander>();
        hander?.ForgotPass();
    }
}
