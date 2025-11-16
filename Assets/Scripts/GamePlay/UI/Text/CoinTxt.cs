public class CoinTxt : TextBase
{
    protected override void PrintText()
    {
        text.SetText(GameManager.Instance.Coin.ToString());
    }


}
