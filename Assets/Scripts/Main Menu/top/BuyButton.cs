using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButron : ButtonBase
{
    private Dictionary<string, ShopItemData> listItem = new Dictionary<string, ShopItemData>();
    [SerializeField] private int index;
    QuatityItemTxt quatityTxt;
    // Start is called before the first frame update
    protected override  void Start()
    {
        base.Start();
        quatityTxt =transform.GetChild(1).GetComponent<QuatityItemTxt>();
        
        foreach (var soItemin in Resources.LoadAll<ShopItemData>("SO"))
        {
            listItem.Add(soItemin.nameItem, soItemin);
        }

    }

  
    public override void OnClick()
    {
        PurcharseItem(index);
    }
    public void PurcharseItem(int index)
    {
      
        switch (index)
        {
            case 0:
                GameMechanics.BuyItemChecking(listItem, "BoostTurn");
                break;
            case 1:
                GameMechanics.BuyItemChecking(listItem, "BoostTime");
                break; 
            case 2:
                GameMechanics.BuyItemChecking(listItem, "BoostGold");
                break;
            case 11:
                GameMechanics.BuyItemChecking(listItem, "BoostActiveHammer");
                break; 
            case 12:
                GameMechanics.BuyItemChecking(listItem, "BoostActiveShuffle");
                break;

        }

    }

}
