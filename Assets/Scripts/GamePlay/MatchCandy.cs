using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatchCandy
{
    // hàm xoa các  object
    public static void DestroyAnfndRefill(CandyVisual[,] candyVisuals, GridManager grid, List<CandyVisual> candies, GameObject[] candyPrefabs)
    {
        foreach (CandyVisual candy in candies)
        {
            int row = candy.Row;
            int col = candy.Colum;

            candyVisuals[row, col] = null;  // clear trong grid 


            Object.Destroy(candy.gameObject);// xóa object


            grid.StartCoroutine(RefillAffterDelay(candyVisuals, grid, candyPrefabs, grid.LocalSize)); //sau khi xoa sẽ thực hiện tạo và lấy đày
        }
    }


    private static IEnumerator RefillAffterDelay(CandyVisual[,] candies, GridManager grid, GameObject[] candyPrefabs, float localSize)
    {
        yield return new WaitForSeconds(0.5f);
        MatchCandy.CollapseColumn(candies, grid);
        MatchCandy.Refill(grid, candies, candyPrefabs, localSize);
        yield return new WaitForSeconds(0.3f);
        MatchCandy.MatchAllCandy(candies, grid, candyPrefabs);

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
            DestroyAnfndRefill(candies, grid, new List<CandyVisual>(allMatches), candyPrefabs);
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

        for (int c = col - 1; c >= 0; c--) // duyet tung cột từ phải sang trái
        {
            //kiểm tra cung kiểu sẽ thưc hiện thêm vào
            if (candies[row, c] != null && candy.TypeCandy == candies[row, c].TypeCandy)
            {

                horizontal.Add(candies[row, c]);

            }
            else break;
        }

        for (int c = col + 1; c < cols; c++) // duyệt từ cột từ trái qua phải
        {
            //kiểm tra cung kiểu sẽ thưc hiện thêm vào
            if (candies[row, c] != null && candy.TypeCandy == candies[row, c].TypeCandy)

            {
                horizontal.Add(candies[row, c]);
            }
            else break;
        }
        if (horizontal.Count >= 3)// số lương thêm phải lơn hơn 3 mỡi thực hiện hợp nhất với các obj cung kiểu
        {
            matchCandies.UnionWith(horizontal);
            GameMechanics.AddScore(horizontal.Count);
        }


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

        // số lương thêm phải lơn hơn 3 mỡi thực hiện hợp nhất với các obj cung kiểu
        if (vertical.Count >= 3)
        {
            matchCandies.UnionWith(vertical);
            GameMechanics.AddScore(vertical.Count);
        }
        return matchCandies;
    }


    //dồn candy lại xuông
    public static void CollapseColumn(CandyVisual[,] candies, GridManager grid)
    {
        // Duyệt từng hàng (row = y)
        for (int x = 0; x < grid.Width; x++)
        {
            int writeRow = 0;
            for (int y = 0; y < grid.Height; y++)
            {
                while (writeRow < y && grid.LevelLayout != null && !grid.LevelLayout[x, writeRow])
                {
                    writeRow++;
                }
                if (grid.LevelLayout != null && !grid.LevelLayout[x, y]) continue;
                if (candies[x, y] != null)
                {
                    if (y != writeRow)
                    {
                        CandyVisual candy = candies[x, y];
                        candies[x, writeRow] = candy;
                        candies[x, y] = null; // Ô cũ (ở trên) bây giờ là ô trống
                        Vector2 pos2D = grid.board.GetWorldPosition(x, writeRow);
                        Vector3 targetPos = grid.transform.position + new Vector3(pos2D.x, pos2D.y, -1);

                        candy.SetPositionGrid(x, writeRow);
                        // Kêu gọi kẹo di chuyển xuống vị trí mới
                        candy.SetPositionCandy(targetPos);
                    }
                    writeRow++; // Tăng vị trí thấp nhất có kẹo lên 1 (lên trên)
                }

            }
        }
    }

    // tạo lại các candy dể lấp đầy grid bằng candy
    public static void Refill(GridManager grid, CandyVisual[,] candies, GameObject[] candyPrefabs, float localSize)
    {
        for (int x = 0; x < grid.Width; x++)
        {
            // Duyệt TỪ TRÊN XUỐNG DƯỚI (y)
            for (int y = grid.Height - 1; y >= 0; y--)
            {
                if (grid.LevelLayout != null && !grid.LevelLayout[x, y]) continue;
                if (candies[x, y] != null) continue; // Bỏ qua nếu đã có kẹo
                Vector2 pos2D = grid.board.GetWorldPosition(x, y);
                int candyTypeID = Random.Range(0, candyPrefabs.Length);
                Vector3 targetPos = grid.transform.position + new Vector3(pos2D.x, pos2D.y, -1);
                Vector3 startPos = grid.transform.position + new Vector3(pos2D.x, grid.Height * (grid.CellSize + grid.Spacing), -1);
                GameObject newCandy = Object.Instantiate(candyPrefabs[candyTypeID], startPos, Quaternion.identity, grid.transform);
                CandyVisual candyVisual = newCandy.GetComponent<CandyVisual>();
                if (candyVisual == null) return;
                candies[x, y] = candyVisual;

                candyVisual.SetScale(localSize);
                candyVisual.SetPositionGrid(x, y);
                candyVisual.SetPositionCandy(targetPos);
                candyVisual.SetGridManager(grid);
            }
        }
    }

}
