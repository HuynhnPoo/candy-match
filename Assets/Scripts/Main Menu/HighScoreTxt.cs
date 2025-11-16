public class HighScoreTxt : TextBase
{
    protected override void PrintText()
    {
        text.SetText(GameManager.Instance.ScoreDown.ToString());
    }


}
