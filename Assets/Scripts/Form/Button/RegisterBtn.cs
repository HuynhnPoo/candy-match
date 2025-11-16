public class RegisterBtn : ButtonBase
{
    private FormHander hander;
    public override void OnClick()
    {
        hander = UIManager.Instance.managerCanvas.GetComponentInChildren<FormHander>();

        hander.Register();
    }
}
