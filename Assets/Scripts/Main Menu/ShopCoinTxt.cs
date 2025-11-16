public class ShopCoinTxt : TextBase
{
    protected override void PrintText()
    {
        text.SetText(GameManager.Instance.CoinDown.ToString());
    }


}
