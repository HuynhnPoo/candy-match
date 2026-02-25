using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleCandy
{
    private const int MAX_SHUFFLE_ATTEMPTS = 5;
    private static void ShuffleElemnets(GridManager grid, CandyVisual[,] candies)
    {
        List<CandyVisual> allCandies = new List<CandyVisual>();

        for (int row = 0; row < grid.Width; row++)
        {
            for (int col = 0; col < grid.Height; col++)
            {
                if (grid.LevelLayout[row, col] && candies != null)
                {
                    allCandies.Add(candies[row, col]);
                    candies[row, col] = null;
                }
            }
        }

        for (int i = 0; i < allCandies.Count; i++)
        {
            int ramdomIndex = UnityEngine.Random.Range(0, allCandies.Count);
            (allCandies[i], allCandies[ramdomIndex]) = (allCandies[ramdomIndex], allCandies[i]);
          
        }

        int candyIndex = 0;
        for (int row = 0; row < grid.Width; row++)
        {
            for (int col = 0; col < grid.Height; col++)
            {
                if (grid.LevelLayout[row, col])
                {
                    if (candyIndex < allCandies.Count)
                    {
                        CandyVisual candyVisual = allCandies[candyIndex];
                      
                        
                        candies[row, col] = candyVisual;
                        Vector2 pos2d = grid.board.GetWorldPosition(row, col);
                        Vector3 target = grid.transform.position + new Vector3(pos2d.x, pos2d.y, -1);

                        candyVisual.SetPositionGrid(row, col);
                        candyVisual.SetPositionCandy(target);
                        candyIndex++;
                    }
                }
            }
        }
    }

    public static IEnumerator ShuffleBoard(GridManager grid, CandyVisual[,] candies, int attempt = 0)
    {
        if (attempt >= MAX_SHUFFLE_ATTEMPTS)
        {
            Debug.LogError($"Shuffle thất bại sau {MAX_SHUFFLE_ATTEMPTS} lần thử. Không thể tìm được nước đi hợp lệ.");
            // Đây là điểm dừng cuối cùng.
            yield break;
        }
        ShuffleElemnets(grid, candies);
        yield return new WaitForSeconds(0.01f);
           ForceCreateMatch(grid, candies);
        if (!MatchCandy.CheckValidMovePossible(grid, candies))
        {
            yield return grid.StartCoroutine(ShuffleBoard(grid, candies, attempt + 1));
        }
        else
        {
            Debug.Log("hien ti ra B");
        }

    }

  static void ForceCreateMatch(GridManager grid, CandyVisual[,] candies)
    {
        for (int row = 0; row < grid.Width-2; row++)
        {
            for (int col = 0; col < grid.Height-2; col++)
            {
                if (col+2 >= grid.Height || row+2 >= grid.Width)
                {
                    // Điều kiện này chỉ là kiểm tra dự phòng, nhưng logic for đã đảm bảo
                    continue;
                }
                if (!grid.LevelLayout[row, col] || !grid.LevelLayout[row, col] || !grid.LevelLayout[row, col]) continue;

                CandyVisual candiesA = candies[row, col];
                CandyVisual candiesB = candies[row + 1, col];
                CandyVisual candiesC = candies[row +2, col+1 ];

                if (candiesA ==null || candiesB ==null|| candiesC==null) continue;

              
                int ramdomType = UnityEngine.Random.Range(0,Enum.GetValues(typeof(CandyType.CandyTypeList)).Length);

                // cung type mới có thể cap nhật nếu khoog nó sẽ cập nhật sai type candy
                candiesA.SetTypeCandy((CandyType.CandyTypeList) ramdomType,true); 
                candiesB.SetTypeCandy((CandyType.CandyTypeList) ramdomType,true);
                candiesC.SetTypeCandy((CandyType.CandyTypeList) ramdomType,true);
                return;

            }
        }
    }
}
