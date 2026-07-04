using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Board
{
    private int width;
    private int height;
    private int cellsize;
    private float spacing;
    private int[,] board;
   
   
    public Board(int width, int height, int cellSize, float spacing)
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellSize;
        this.spacing = spacing;

        board = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                board[x, y] = Random.Range(0, 5); // ngẫu nhiên game object được sinh ra
            }
        }

    }

    public int GetCandy(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) return board[x, y]; //laays vị trí và trả về game object
        return -1;
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        float step = cellsize + spacing;

        float totalWidth = (width - 1) * step;
        float totalHeight = (height - 1) * step;

        float offsetX = -totalWidth / 2f;
        float offsetY = -totalHeight / 2f;

        return new Vector2(
            x * step + offsetX,
            y * step + offsetY
        );
    }


    public int CountCurrentColorBombs(CandyVisual[,] candyVisual,int row,int col)
    {
       
        int countBomb = 0;
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                //  Debug.Log("hien thi " + candyVisual[x,y]);
                if (candyVisual[x,y] != null && candyVisual[x, y].TypeCandy == CandyType.CandyTypeList.BOMB_CANDY)
                {
                    countBomb++;
                //    Debug.Log(countBomb);
                }
            }
        }
        return countBomb;
    }


    // ham swap candy
    public void Swap(CandyVisual[,] candies, GridManager grid, int rowA, int colA, int rowB, int colB)
    {
        CandyVisual candyA = candies[rowA, colA]; // vị trí candy 1 trong manng

        CandyVisual candyB = candies[rowB, colB];


        if( candyA == null || candyB == null )
        {
            Debug.LogWarning(" 1 trong 2 candy khong di chuyen do vat can");
             return;
        }
      //  Debug.Log("hien thi ra candy b "+ candyB.name);

        candies[rowA, colA] = candyB;
        candies[rowB, colB] = candyA;

        Vector2 posA = GetWorldPosition(rowA, colA);
        Vector2 posB = GetWorldPosition(rowB, colB);

        Vector3 candyPosA = grid.transform.position + new Vector3(posA.x, posA.y, -1);
        Vector3 candyPosB = grid.transform.position + new Vector3(posB.x, posB.y, -1);


        candyA.SetPositionCandy(candyPosB);
        candyA.SetPositionGrid(rowB, colB);

        candyB.SetPositionCandy(candyPosA);
        candyB.SetPositionGrid(rowA, colA);


        bool isMatched = grid.CheckMatchesForSwap(rowA, colA, rowB, colB);
        if (!isMatched)
        {
            grid.StartCoroutine(RoutineRevertSpwan(candies, grid, rowA, colA, rowB, colB, candyA, candyB));
        }

    }


    IEnumerator RoutineRevertSpwan(CandyVisual[,] candies, GridManager grid, int rowA, int colA, int rowB, int colB, CandyVisual candyA, CandyVisual candyB)
    {
        yield return new WaitForSeconds(0.5f); // sau 0.5 se thuc hien lai
        RevertSwap(candies, grid, rowA, colA, rowB, colB, candyA, candyB);
    }

    // ham dổi nguoc lại cua candy
    void RevertSwap(CandyVisual[,] candies, GridManager grid, int rowA, int colA, int rowB, int colB, CandyVisual candyA, CandyVisual candyB)
    {
        candies[rowA, colA] = candyA;
        candies[rowB, colB] = candyB;

        Vector2 posA = GetWorldPosition(rowA, colA);
        Vector2 posB = GetWorldPosition(rowB, colB);

        Vector3 candyPosA = grid.transform.position + new Vector3(posA.x, posA.y, -1);
        Vector3 candyPosB = grid.transform.position + new Vector3(posB.x, posB.y, -1);


        candyA.GetComponent<CandyVisual>().SetPositionCandy(candyPosA);
        candyA.GetComponent<CandyVisual>().SetPositionGrid(rowA, colA);

        candyB.GetComponent<CandyVisual>().SetPositionCandy(candyPosB);
        candyB.GetComponent<CandyVisual>().SetPositionGrid(rowB, colB);


    }

}
