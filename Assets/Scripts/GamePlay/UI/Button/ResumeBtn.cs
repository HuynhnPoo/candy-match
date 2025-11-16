public class ResumeBtn : ButtonBase
{
    public override void OnClick()
    {
        GameManager.Instance.Pausing(GameManager.Instance.IsPaused);
    }
}
