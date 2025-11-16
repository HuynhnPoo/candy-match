public class PauseBtn : ButtonBase
{
    public override void OnClick()
    {
        GameManager.Instance.Pausing(GameManager.Instance.IsPaused);
    }
}
