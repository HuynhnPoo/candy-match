using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBoostBtn : ButtonBase
{
    [SerializeField] private string nameItem; 
    public override void OnClick()
    {
        BoostItemManager.InitializeActive();
        IteminventoryManager.UseItem(nameItem);
    }
}
