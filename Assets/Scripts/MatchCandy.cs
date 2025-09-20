using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public static class MatchCandy
{
    public static void DestroyAnfndRefill( CandyVisual[,] candyVisuals,GridManager grid,List<CandyVisual> candies, GameObject[] candyPrefabs)
    {
        foreach (CandyVisual candy in candies)
        {
            int row = candy.Row;
            int col = candy.Colum;

            // clear trong grid
            candyVisuals[row, col] = null;

            // huỷ object
           Object.Destroy(candy.gameObject);


            grid.StartCoroutine(RefillAffterDelay(candyVisuals, grid,candyPrefabs));
        }
    }
    private static IEnumerator RefillAffterDelay(CandyVisual[,] candies,GridManager grid,GameObject[] candyPrefabs)
    {
        yield return new WaitForSeconds(0.5f);
        MatchCandy.CollapseColumn(candies, grid.board,grid.transform,grid.Height,grid.Width);
        MatchCandy.Refill(grid, candies,candyPrefabs);
        yield return new WaitForSeconds(0.3f);
        MatchCandy.MatchAllCandy(candies,grid,candyPrefabs);

    }

    public static void MatchAllCandy(CandyVisual[,] candies, GridManager grid, GameObject[] candyPrefabs)
    {

        HashSet<CandyVisual> allMatches = new HashSet<CandyVisual>();

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                CandyVisual candy = candies[x, y];
                if (candy == null) continue;
                HashSet<CandyVisual> match = MatchCandy.FindAllMacth(candies, grid.Width, grid.Height, x, y);
                allMatches.UnionWith(match);
            }
        }

        if (allMatches.Count >= 3)// nếu lơn hơn 3 sẽ thục hiện xóa
        {
            DestroyAnfndRefill(candies,grid,new List<CandyVisual>(allMatches),candyPrefabs);
        }

    }

    public static HashSet<CandyVisual> FindAllMacth(CandyVisual[,] candies, int width, int height, int row, int col)
    {

        HashSet<CandyVisual> allMatches = new HashSet<CandyVisual>();
        allMatches.UnionWith(FindHorizontalMatch(candies, row, col, width));
        allMatches.UnionWith(FindVerticalMatch(candies, row, col, height));

        return allMatches;
    }

    private static HashSet<CandyVisual> FindHorizontalMatch(CandyVisual[,] candies, int row, int col, int cols)
    {

        HashSet<CandyVisual> matchCandies = new HashSet<CandyVisual>();
        CandyVisual candy = candies[row, col];
        if (candy == null) return matchCandies;
        List<CandyVisual> horizontal = new List<CandyVisual>() { candy };

        for (int c = col - 1; c >= 0; c--) // duyet tu duoi len
        {

            if (candies[row, c] != null && candy.TypeCandy == candies[row, c].TypeCandy)
            {

                horizontal.Add(candies[row, c]);

            }
            else break;
        }

        for (int c = col + 1; c < cols; c++) // duyệt từ tren xuông
        {

            if (candies[row, c] != null && candy.TypeCandy == candies[row, c].TypeCandy)

            {
                horizontal.Add(candies[row, c]);
            }
            else break;
        }
        if (horizontal.Count >= 3)

            matchCandies.UnionWith(horizontal);

        Debug.Log(matchCandies.Count);
        return matchCandies;


    }
    private static HashSet<CandyVisual> FindVerticalMatch(CandyVisual[,] candies, int row, int col, int rows)
    {
        HashSet<CandyVisual> matchCandies = new HashSet<CandyVisual>();
        CandyVisual candy = candies[row, col];
        if (candy == null) return matchCandies;

        List<CandyVisual> vertical = new List<CandyVisual>() { candy };

        for (int r = row - 1; r >= 0; r--)
        {
            if (candies[r, col] != null && candies[r, col].TypeCandy == candy.TypeCandy)
            {
                vertical.Add(candies[r, col]);
            }
            else break;

        }
        for (int r = row + 1; r < rows; r++)
        {
            if (candies[r, col] != null && candies[r, col].TypeCandy == candy.TypeCandy)
            {
                vertical.Add(candies[r, col]);

            }
            else break;
        }
        if (vertical.Count >= 3) matchCandies.UnionWith(vertical);
        Debug.Log(matchCandies.Count);
        return matchCandies;
    }

    public static void CollapseColumn(CandyVisual[,] candies, Board board, Transform posCandies, int width, int height)
    {
        // Duyệt từng hàng (row = y)
        for (int row = 0; row < height; row++)
        {
            int writeCol = 0; // vị trí trái nhất để đặt candy

            // duyệt từ trái qua phải
            for (int col = 0; col < width; col++)
            {
                if (candies[col, row] != null)
                {
                    if (col != writeCol)
                    {
                        CandyVisual candy = candies[col, row];

                        // đưa candy về cột "trái" writeCol
                        candies[writeCol, row] = candy;
                        candies[col, row] = null;

                        Vector2 pos = board.GetWorldPosition(writeCol, row);
                        Vector3 posCandy = posCandies.position + new Vector3(pos.x, pos.y, -1);

                        candy.SetPositionGrid(writeCol, row);
                        candy.SetPositionCandy(posCandy);

                    }
                    writeCol++;
                }
            }
        }
    }


    public static void Refill(GridManager grid,CandyVisual[,] candies,GameObject[] candyPrefabs)
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                if (candies[x, y] != null) continue;
                Debug.Log("thuc hien hàm re fill");
                Vector2 pos2D = grid.board.GetWorldPosition(x, y);
                int candy = grid.board.GetCandy(x, y);
                Vector3 candyPos = grid.transform.position + new Vector3(pos2D.x, pos2D.y, -1);
                Vector3 candyPosStart = grid.transform.position + new Vector3(pos2D.x,grid.Height * (grid.CellSize + grid.Spacing), -1);
                GameObject newCandy =Object.Instantiate(candyPrefabs[candy], candyPosStart, Quaternion.identity,grid.transform);
                //Debug.Log("debug gia tra cua can laf gif "+candy);

                CandyVisual candyVisual = newCandy.GetComponent<CandyVisual>();
                if (candyVisual == null) return;
                candies[x, y] = candyVisual;
                candyVisual.SetPositionGrid(x, y);
                candyVisual.SetPositionCandy(candyPos);

                candyVisual.SetGridManager(grid);
            }
        }
    }

}
