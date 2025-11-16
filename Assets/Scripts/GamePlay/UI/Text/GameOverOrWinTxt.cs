public class GameOverOrWinTxt : TextBase
{
    protected override void PrintText()
    {
        text.SetText(GameManager.Instance.StatusGameStr);
    }
}
