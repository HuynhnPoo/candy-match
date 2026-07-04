using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Candy Crush/Level Database", order = 0)]
public class LevelDatabase : ScriptableObject
{
    [Header("All Levels")]
    public List<LevelLayoutData> levels = new List<LevelLayoutData>();

    public LevelLayoutData GetLevelLayout(int levelNumber)
    {
       
        foreach (LevelLayoutData level in levels)
        {
            if (level.levelNumber == levelNumber)
            {

                Debug.Log("hien thi ra  level number " + levelNumber + "______" + level.levelNumber);
                Debug.Log(level + "hien thi lvel " + level.levelName);
                return level;
            }
        }

        Debug.LogWarning($"Level {levelNumber} not found! Returning default level.");
        return levels.Count > 0 ? levels[0] : null;
    }

    public void GetTotalLevels(int wight,int height)
    {
        Debug.Log($"wight: {wight} và height {height} ");
    }
}