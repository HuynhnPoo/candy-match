using System;
using UnityEngine;

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
        BOMB_CANDY
    }
}

public static class CandyName
{
    public static void LoadName(string name, CandyVisual typeCandy)
    {
        if (name.Equals(CandyType.CandyTypeList.RED.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            typeCandy.TypeCandy = CandyType.CandyTypeList.RED;
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
        } else if (name.Equals(CandyType.CandyTypeList.BOMB_CANDY.ToString(), StringComparison.OrdinalIgnoreCase))
        {

            typeCandy.TypeCandy = CandyType.CandyTypeList.BOMB_CANDY;
        }
        else
        {
            Debug.Log("ten nay khog co" + name);
        }

    }

}
