public class ScoreTxt : TextBase
{
    public int indexSelect;
    protected override void PrintText()
    {
        if (indexSelect == 0)
        {
            this.text.SetText(GameManager.Instance.Score.ToString());
        }
        else if (indexSelect == 1)
        {
            {
                this.text.SetText(GameManager.Instance.HighScore.ToString());
            }
        }


    }
}
