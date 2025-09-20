using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CandyType
{
    public CandyTypeList candy = CandyTypeList.RED;

    public enum CandyTypeList
    {
        RED,
        GREEN,
        BLUE,
        YELLOW,
        PINK,

    }
}

public static class CandyName
{
    public static void LoadName(string name,CandyVisual typeCandy)
    {
        if (name.Equals(CandyType.CandyTypeList.RED.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            typeCandy.TypeCandy=CandyType.CandyTypeList.RED;
        }
        else if (name.Equals(CandyType.CandyTypeList.GREEN.ToString(), StringComparison.OrdinalIgnoreCase))
        {

            typeCandy.TypeCandy = CandyType.CandyTypeList.GREEN;
        }
        else if (name.Equals(CandyType.CandyTypeList.BLUE.ToString(), StringComparison.OrdinalIgnoreCase))
        {

            typeCandy.TypeCandy = CandyType.CandyTypeList.BLUE;
        }
        else if (name.Equals(CandyType.CandyTypeList.YELLOW.ToString(), StringComparison.OrdinalIgnoreCase))
        {

            typeCandy.TypeCandy = CandyType.CandyTypeList.YELLOW;
        }
        else if (name.Equals(CandyType.CandyTypeList.PINK.ToString(), StringComparison.OrdinalIgnoreCase))
        {

            typeCandy.TypeCandy = CandyType.CandyTypeList.PINK;
        }
        else
        {
            Debug.Log("ten nay khog co" + name);
        }

    }

}
