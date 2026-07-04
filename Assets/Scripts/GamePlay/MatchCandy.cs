using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public static class MatchCandy
{
    // hàm xoa các  object
    public static void DestroyAnfndRefill(CandyVisual[,] candyVisuals, GridManager grid, List<CandyVisual> candies, GameObject[] candyPrefabs)
    {
        foreach (CandyVisual candy in candies)
        {
            if (candy == null) continue;
            int row = candy.Row;
            int col = candy.Colum;

            candyVisuals[row, col] = null;  // clear trong grid 


            candy.DestroyCandy(); // tạo hiệu úng nổ
            Object.Destroy(candy.gameObject);// xóa object

        }
        grid.StartCoroutine(RefillAffterDelay(candyVisuals, grid, candyPrefabs, grid.LocalSize)); //sau khi xoa sẽ thực hiện tạo và lấy đày
    }


    private static IEnumerator RefillAffterDelay(CandyVisual[,] candies, GridManager grid, GameObject[] candyPrefabs, float localSize)
    {
        yield return new WaitForSeconds(0.5f);
        CollapseColumn(candies, grid);
        Refill(grid, candies, candyPrefabs, localSize);
        yield return new WaitForSeconds(0.3f);
        MatchAllCandyAffterRefill(candies, grid, candyPrefabs);
        yield return new WaitForSeconds(0.8f);

        

    }
  
    public static void MatchAllCandyAffterRefill(CandyVisual[,] candies, GridManager grid, GameObject[] candyPrefabs)
    {

        HashSet<CandyVisual> allMatches = new HashSet<CandyVisual>();

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                CandyVisual candy = candies[x, y];
                if (candy == null) continue;
                HashSet<CandyVisual> match = FindAllMacth(candies, grid.Width, grid.Height, x, y);
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

        HashSet<CandyVisual> horizontalMatch = FindVerticalMatch(candies, row, col, height);
        HashSet<CandyVisual> verticalMatch = FindHorizontalMatch(candies, row, col, width);

        // Chỉ kiểm tra khi ô hiện tại là ô TRÁI NHẤT của match ngang
        if (horizontalMatch.Count >= 3)
        {

            //  Debug.Log(horizontalMatch.Count + " thuc hien kiem tra ngang tai [" + col + "]");

            CheckLineColorBombLocal(candies, horizontalMatch, true);
            allMatches.UnionWith(horizontalMatch);

        }

        // Chỉ kiểm tra khi ô hiện tại là ô TRÊN CÙNG của match dọc
        if (verticalMatch.Count >= 3)
        {
            // Debug.Log(verticalMatch.Count + " thuc hien kiem tra doc tai [" + row + "]");
            CheckLineColorBombLocal(candies, verticalMatch, false);
            allMatches.UnionWith(verticalMatch);
        }


        return allMatches;
    }

    /*  static void CheckColorBomb(GridManager gridManager, CandyVisual[,] candies, HashSet<CandyVisual> match)
       {


           Vector2Int vertical = gridManager.lastSwapA;
           Vector2Int horizontal = gridManager.lastSwapB;

           Debug.Log("hien ra a b" + vertical + horizontal);

           if (vertical.x == horizontal.x) // kiểm tra theo hang dọc xem có color bomb không
               GameMechanics.CheckLineColorBomb(gridManager,candies,match,vertical,horizontal,gridManager.Height);

           if (vertical.y == horizontal.y) // kiểm tra theo hàng nagng xme có color bomb không
               GameMechanics.CheckHorizontalColorBomb(gridManager, candies, match, vertical, horizontal, gridManager.Width);

       }*/

    static void CheckLineColorBombLocal(CandyVisual[,] candies, HashSet<CandyVisual> matchedCandies, bool isHorizontal)
    {
        int maxRows = candies.GetLength(0);
        int maxCols = candies.GetLength(1);

        // Lấy tất cả vị trí của các candy đã match
        HashSet<(int row, int col)> matchedPositions = new HashSet<(int, int)>();

        for (int r = 0; r < maxRows; r++)
        {
            for (int c = 0; c < maxCols; c++)
            {
                if (candies[r, c] != null && matchedCandies.Contains(candies[r, c]))
                {
                    matchedPositions.Add((r, c));
                }
            }
        }

        // Kiểm tra các ô bên cạnh
        foreach (var pos in matchedPositions)
        {
            if (isHorizontal)
            {
                // Match ngang -> kiểm tra trên/dưới
                CheckAndTrigger(pos.row - 1, pos.col, candies, matchedCandies);
                CheckAndTrigger(pos.row + 1, pos.col, candies, matchedCandies);
            }
            else
            {
                // Match dọc -> kiểm tra trái/phải
                CheckAndTrigger(pos.row, pos.col - 1, candies, matchedCandies);
                CheckAndTrigger(pos.row, pos.col + 1, candies, matchedCandies);
            }
        }
    }

    private static void CheckAndTrigger(int row, int col, CandyVisual[,] candies, HashSet<CandyVisual> matchedCandies)
    {
        int maxRows = candies.GetLength(0);
        int maxCols = candies.GetLength(1);

        if (row < 0 || row >= maxRows || col < 0 || col >= maxCols)
            return;

        CandyVisual target = candies[row, col];

        if (target != null && !matchedCandies.Contains(target) && target.TypeCandy == CandyType.CandyTypeList.BOMB_CANDY)
        {
            Debug.Log($"Kích hoạt bomb tại [{row},{col}] - Type: {target.TypeCandy}");

            // Truyền thêm matchedCandies để các candy bị phá hủy được thêm vào

            ImplementExplorePlus(candies, row, col, matchedCandies);
        }
    }

    static void ImplementExplorePlus(CandyVisual[,] candies, int currentRow, int currentCol, HashSet<CandyVisual> matchedCandies) // thực hiên xoa các ô theo dấu cọnog
    {
        int maxRow = candies.GetLength(0);
        int maxCol = candies.GetLength(1);

        Debug.Log("Thực hiện nổ theo dấu cộng tại [" + currentRow + "," + currentCol + "]");

        // Phá hủy hàng ngang
        for (int col = 0; col < maxCol; col++)
        {
            if (candies[currentRow, col] != null)
            {
                matchedCandies.Add(candies[currentRow, col]); // THÊM VÀO matchedCandies
            }
        }

        // Phá hủy hàng dọc
        for (int row = 0; row < maxRow; row++)
        {
            if (candies[row, currentCol] != null)
            {
                matchedCandies.Add(candies[row, currentCol]); // THÊM VÀO matchedCandies
            }
        }
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
            if (candies[row, c] != null && candy.TypeCandy != CandyType.CandyTypeList.BOMB_CANDY && candy.TypeCandy == candies[row, c].TypeCandy)
            {

                horizontal.Add(candies[row, c]);

            }
            else break;
        }

        for (int c = col + 1; c < cols; c++) // duyệt từ cột từ trái qua phải
        {
            //kiểm tra cung kiểu sẽ thưc hiện thêm vào
            if (candies[row, c] != null && candy.TypeCandy != CandyType.CandyTypeList.BOMB_CANDY && candy.TypeCandy == candies[row, c].TypeCandy)

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
            if (candies[r, col] != null && candy.TypeCandy != CandyType.CandyTypeList.BOMB_CANDY && candies[r, col].TypeCandy == candy.TypeCandy)
            {

                vertical.Add(candies[r, col]);
            }
            else break;

        }
        for (int r = row + 1; r < rows; r++)
        {
            if (candies[r, col] != null && candy.TypeCandy != CandyType.CandyTypeList.BOMB_CANDY && candies[r, col].TypeCandy == candy.TypeCandy)
            {
                vertical.Add(candies[r, col]);


            }
            else break;
        }

        // số lương thêm phải lơn hơn 3 mỡi thực hiện hợp nhất với các obj cung kiểu
        if (vertical.Count >= 3)
        {
            matchCandies.UnionWith(vertical);
            //  CheckColorBombVertical(,)
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
                while (writeRow < grid.Height && writeRow < y && grid.LevelLayout != null && !grid.LevelLayout[x, writeRow])
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

   /* // tạo lại các candy dể lấp đầy grid bằng candy
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

                int newIndexCandy = 0;
                float ramdomAAA = Random.value;
                if (grid.board.CountCurrentColorBombs(candies, grid.Width, grid.Height) < 2 && ramdomAAA < 50f)

                {
                    newIndexCandy = candyPrefabs.Length - 1;
                }
                else
                {
                    //int candyTypeID = Random.Range(0, candyPrefabs.Length - 1);
                    newIndexCandy = Random.Range(0, candyPrefabs.Length - 1);
                }


                Vector3 targetPos = grid.transform.position + new Vector3(pos2D.x, pos2D.y, -1);
                Vector3 startPos = grid.transform.position + new Vector3(pos2D.x, grid.Height * (grid.CellSize + grid.Spacing), -1);
                GameObject newCandy = Object.Instantiate(candyPrefabs[newIndexCandy], startPos, Quaternion.identity, grid.transform);
                CandyVisual candyVisual = newCandy.GetComponent<CandyVisual>();
                if (candyVisual == null) return;
                candies[x, y] = candyVisual;

                candyVisual.SetScale(localSize);
                candyVisual.SetPositionGrid(x, y);
                candyVisual.SetPositionCandy(targetPos);
                candyVisual.SetGridManager(grid);
            }
        }
    }*/


    //Nếu CheckValidMatch của bạn làm nhiệm vụ của CheckForImmediateMatches (tìm match 3 hiện tại): Mã của bạn là đúng.
    public static bool CheckValidMatch(GridManager grid, CandyVisual[,] candies)
    {
        for (int row = 0; row < grid.Width; row++)
        {
            for (int col = 0; col < grid.Height; col++)
            {
                HashSet<CandyVisual> matchCandy = FindAllMacth(candies, grid.Width, grid.Height, row, col);
                if (matchCandy.Count >= 3)
                    return true;
            }
        }
        return false;
    }

    public static bool CheckValidMovePossible(GridManager grid, CandyVisual[,] candies)
    {
        // Lặp qua tất cả các ô trong lưới
        for (int row = 0; row < grid.Width; row++)
        {
            for (int col = 0; col < grid.Height; col++)
            {
                if (candies[row, col] == null) continue;

                // kiem tra hoán đổi bên phải
                if (col < grid.Height - 1 && candies[row, col + 1] != null && CheckSwapMove(candies, row, col, row, col + 1)) return true;

                // kiểm tra  hoán đổi bên dưới
                if (row < grid.Width - 1 && candies[row + 1, col] != null && CheckSwapMove(candies, row, col, row + 1, col)) return true;
            }
        }
        return false;
    }

    // Hàm phụ trợ rút gọn: Hoán đổi tạm thời, kiểm tra, và hoàn tác
    private static bool CheckSwapMove(CandyVisual[,] candies, int row1, int col1, int row2, int col2)
    {
        (candies[row1, col1], candies[row2, col2]) = (candies[row2, col2], candies[row1, col1]); //hoán đổi tạm thời

        bool matchFound = MatchCount(candies, row1, col1) >= 3 || MatchCount(candies, row2, col2) >= 3; //kiêm tra vị trí 2 viên kẹo

        (candies[row1, col1], candies[row2, col2]) = (candies[row2, col2], candies[row1, col1]); //hoán đổi lại ngược lại

        return matchFound;
    }

    private static int MatchCount(CandyVisual[,] candies, int row, int col)
    {
        return FindAllMacth(candies, candies.GetLength(0), candies.GetLength(1), row, col).Count;
    }


    // thục hien xóa hang khi clikc vào row
    public static void ClearRow(CandyVisual[,] candies, GridManager grid, int currentRow, GameObject[] candyPrefabs)
    {
        for (int col = 0; col < candies.GetLength(0); col++)
        {
            CandyVisual candy = candies[currentRow, col]; // cho  gắn từng obj cho candy
            if (candy == null) return;

            Object.Destroy(candy.gameObject); //thực hiện xóa

            candies[currentRow, col] = null;

        }
        grid.StartCoroutine(RefillAffterDelay(candies, grid, candyPrefabs, grid.LocalSize)); //lấp đầy bảng
    }


    // thực hiện thay đổi candy bằng candy khác
    public static void ChangeCandy(CandyVisual[,] candies, int x1, int y1, int x2, int y2)
    {
        CandyVisual candyA = candies[x1, y1]; // lấy vị trí cua 2 candy
        CandyVisual candyB = candies[x2, y2];

        SpriteRenderer spriteRenderA = candyA.transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderB = candyB.transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (candyA == null || candyB == null) return;

        Sprite tempSprite = spriteRenderA.sprite;

        spriteRenderB.sprite = tempSprite;
        spriteRenderB.color= Color.white;
       
        candyB.SetTypeCandy(candyA.TypeCandy,false);

    }

    public static void Refill(GridManager grid, CandyVisual[,] candies,
    GameObject[] candyPrefabs, float localSize)
    {
        int totalNormal = candyPrefabs.Length - 1; // bỏ BOMB

        // ── Bước 1: Spawn tất cả candy ngẫu nhiên vào ô trống ──
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = grid.Height - 1; y >= 0; y--)
            {
                if (grid.LevelLayout != null && !grid.LevelLayout[x, y]) continue;
                if (candies[x, y] != null) continue;

                // Chọn màu không tạo match ngay
                int idx = GetNonMatchIndex(candies, grid, x, y, totalNormal);

                Vector2 pos2D = grid.board.GetWorldPosition(x, y);
                Vector3 targetPos = grid.transform.position
                    + new Vector3(pos2D.x, pos2D.y, -1);
                Vector3 startPos = grid.transform.position
                    + new Vector3(pos2D.x,
                        grid.Height * (grid.CellSize + grid.Spacing), -1);

                GameObject newCandy = Object.Instantiate(
                    candyPrefabs[idx], startPos, Quaternion.identity, grid.transform);
                CandyVisual cv = newCandy.GetComponent<CandyVisual>();
                if (cv == null) continue;

                candies[x, y] = cv;
                cv.SetScale(localSize);
                cv.SetPositionGrid(x, y);
                cv.SetPositionCandy(targetPos);
                cv.SetGridManager(grid);
            }
        }

        // ── Bước 2: Sau khi fill xong, đảm bảo có nước đi ──
        FixBoardToHaveValidMove(candies, grid, candyPrefabs);
    }

    /// <summary>
    /// Chọn màu ngẫu nhiên KHÔNG tạo match 3 ngay tại (x,y)
    /// </summary>
    private static int GetNonMatchIndex(
        CandyVisual[,] candies, GridManager grid,
        int x, int y, int totalNormal)
    {
        List<int> pool = new List<int>();
        for (int i = 0; i < totalNormal; i++) pool.Add(i);

        // Xáo pool để random
        for (int k = pool.Count - 1; k > 0; k--)
        {
            int r = Random.Range(0, k + 1);
            (pool[k], pool[r]) = (pool[r], pool[k]);
        }

        foreach (int idx in pool)
        {
            if (!WouldCauseImmediateMatch(candies, grid, x, y, idx))
                return idx;
        }

        // Không tránh được match thì random bình thường
        return Random.Range(0, totalNormal);
    }

    /// <summary>
    /// Kiểm tra nếu đặt candy idx vào (x,y) có tạo match 3 ngay không
    /// </summary>
    private static bool WouldCauseImmediateMatch(
        CandyVisual[,] candies, GridManager grid,
        int x, int y, int candyTypeIdx)
    {
        var type = (CandyType.CandyTypeList)candyTypeIdx;

        // Kiểm tra ngang (theo x)
        int h = 1;
        for (int i = x - 1; i >= 0
            && candies[i, y] != null
            && candies[i, y].TypeCandy == type; i--) h++;
        for (int i = x + 1; i < grid.Width
            && candies[i, y] != null
            && candies[i, y].TypeCandy == type; i++) h++;
        if (h >= 3) return true;

        // Kiểm tra dọc (theo y)
        int v = 1;
        for (int j = y - 1; j >= 0
            && candies[x, j] != null
            && candies[x, j].TypeCandy == type; j--) v++;
        for (int j = y + 1; j < grid.Height
            && candies[x, j] != null
            && candies[x, j].TypeCandy == type; j++) v++;
        if (v >= 3) return true;

        return false;
    }

    /// <summary>
    /// Sau khi refill xong, nếu không có nước đi
    /// → tìm và sửa TỐI THIỂU số ô để tạo ra ít nhất 1 nước đi
    /// </summary>
    private static void FixBoardToHaveValidMove(
        CandyVisual[,] candies, GridManager grid, GameObject[] candyPrefabs)
    {
        if (CheckValidMovePossible(grid, candies)) return;

        Debug.Log("⚠️ Board chưa có nước đi, đang tạo...");

        int totalNormal = candyPrefabs.Length - 1;

        // Duyệt từng ô, thử tạo pattern nước đi tại đó
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                if (candies[x, y] == null) continue;
                if (candies[x, y].TypeCandy == CandyType.CandyTypeList.BOMB_CANDY) continue;

                var baseType = candies[x, y].TypeCandy;

                // Pattern ngang: [A][A][?][A] → swap ? với A tạo AAA
                // Thử tạo pattern: x và x+1 cùng màu, 
                // rồi x+3 hoặc x-1 cùng màu → swap vào x+2
                if (TryForceHorizontalPattern(candies, grid, candyPrefabs, x, y, baseType))
                    return;

                // Pattern dọc tương tự
                if (TryForceVerticalPattern(candies, grid, candyPrefabs, x, y, baseType))
                    return;
            }
        }

        Debug.LogWarning("❌ Không thể fix board!");
    }

    /// <summary>
    /// Tạo pattern ngang có thể swap: A A _ A hoặc A _ A A
    /// </summary>
    private static bool TryForceHorizontalPattern(
        CandyVisual[,] candies, GridManager grid,
        GameObject[] candyPrefabs,
        int x, int y, CandyType.CandyTypeList type)
    {
        // Pattern: [x][x+1] cùng màu type
        // → cần [x+3] hoặc [x-1] cùng màu để swap vào [x+2]
        if (x + 1 < grid.Width && candies[x + 1, y] != null)
        {
            // Ép x+1 cùng màu x
            ChangeType(candies[x + 1, y], type, (int)type, candyPrefabs);

            // Bây giờ tìm ô swap được: x+3 hoặc x-1
            if (x + 3 < grid.Width && candies[x + 3, y] != null
                && candies[x + 3, y].TypeCandy != CandyType.CandyTypeList.BOMB_CANDY)
            {
                ChangeType(candies[x + 3, y], type, (int)type, candyPrefabs);
                if (CheckValidMovePossible(grid, candies))
                {
                    Debug.Log($"✅ Fix ngang pattern1 tại ({x},{y})");
                    return true;
                }
            }

            if (x - 1 >= 0 && candies[x - 1, y] != null
                && candies[x - 1, y].TypeCandy != CandyType.CandyTypeList.BOMB_CANDY)
            {
                ChangeType(candies[x - 1, y], type, (int)type, candyPrefabs);
                if (CheckValidMovePossible(grid, candies))
                {
                    Debug.Log($"✅ Fix ngang pattern2 tại ({x},{y})");
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Tạo pattern dọc có thể swap
    /// </summary>
    private static bool TryForceVerticalPattern(
        CandyVisual[,] candies, GridManager grid,
        GameObject[] candyPrefabs,
        int x, int y, CandyType.CandyTypeList type)
    {
        if (y + 1 < grid.Height && candies[x, y + 1] != null)
        {
            ChangeType(candies[x, y + 1], type, (int)type, candyPrefabs);

            if (y + 3 < grid.Height && candies[x, y + 3] != null
                && candies[x, y + 3].TypeCandy != CandyType.CandyTypeList.BOMB_CANDY)
            {
                ChangeType(candies[x, y + 3], type, (int)type, candyPrefabs);
                if (CheckValidMovePossible(grid, candies))
                {
                    Debug.Log($"✅ Fix dọc pattern1 tại ({x},{y})");
                    return true;
                }
            }

            if (y - 1 >= 0 && candies[x, y - 1] != null
                && candies[x, y - 1].TypeCandy != CandyType.CandyTypeList.BOMB_CANDY)
            {
                ChangeType(candies[x, y - 1], type, (int)type, candyPrefabs);
                if (CheckValidMovePossible(grid, candies))
                {
                    Debug.Log($"✅ Fix dọc pattern2 tại ({x},{y})");
                    return true;
                }
            }
        }
        return false;
    }

    private static void ChangeType(
        CandyVisual candy,
        CandyType.CandyTypeList newType,
        int prefabIdx,
        GameObject[] candyPrefabs)
    {
        candy.TypeCandy = newType;
        candy.gameObject.name = newType.ToString();

        if (prefabIdx >= 0 && prefabIdx < candyPrefabs.Length - 1)
        {
            SpriteRenderer src = candyPrefabs[prefabIdx]
                .GetComponentInChildren<SpriteRenderer>();
            SpriteRenderer dst = candy
                .GetComponentInChildren<SpriteRenderer>();
            if (src != null && dst != null)
                dst.sprite = src.sprite;
        }
    }

}
