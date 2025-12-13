using UnityEngine;

public class ShopCoinTxt : TextBase
{
    protected override void PrintText()
    {
        /* GameManager.Instance.Coin = PlayerPrefs.GetInt(StringManager.coinSaveStr);
         if (GameManager.Instance.CoinDown != GameManager.Instance.Coin)
         {
             GameManager.Instance.Coin = GameManager.Instance.CoinDown;
             text.SetText(GameManager.Instance.Coin.ToString());
         }*/
        text.SetText(GameManager.Instance.Coin.ToString());
    }
}



