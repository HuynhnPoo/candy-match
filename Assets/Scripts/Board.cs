using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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


                board[x, y] = Random.Range(0, 5);
                // Debug.Log("hien thi ra " + i + "  " + j);
            }
        }

    }

    public int GetCandy(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) return board[x, y];
        return -1;
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        float step = this.cellsize + this.spacing;
        return new Vector2(y * step, x * step);
    }


   public void Swap(CandyVisual[,] candies, GridManager grid, int rowA, int colA, int rowB, int colB)
    {
        CandyVisual candyA = candies[rowA, colA];
        CandyVisual candyB = candies[rowB, colB];

        candies[rowA, colA] = candyB;
        candies[rowB, colB] = candyA;

        Vector2 posA = GetWorldPosition(rowA, colA);
        Vector2 posB = GetWorldPosition(rowB, colB);

        Vector3 candyPosA = grid.transform.position + new Vector3(posA.x, posA.y, -1);
        Vector3 candyPosB = grid.transform.position + new Vector3(posB.x, posB.y, -1);


        candyA.GetComponent<CandyVisual>().SetPositionCandy(candyPosB);
        candyB.GetComponent<CandyVisual>().SetPositionCandy(candyPosA);

        candyA.GetComponent<CandyVisual>().SetPositionGrid(rowB, colB);
        candyB.GetComponent<CandyVisual>().SetPositionGrid(rowA, colA);


        bool isMatched = grid.CheckMatchesForSwap(rowA, colA, rowB, colB);
        if (!isMatched)
        {
            grid.StartCoroutine(RoutineRevertSpwan(candies, grid,colA, rowA, colB, rowB, candyA, candyB));
        }

    }


    IEnumerator RoutineRevertSpwan(CandyVisual[,] candies,GridManager grid, int rowA, int colA, int rowB, int colB, CandyVisual candyA, CandyVisual candyB)
    {
        yield return new WaitForSeconds(0.5f);
        RevertSwap(candies,grid, rowA, colA, rowB, colB, candyA, candyB);
    }

    void RevertSwap(CandyVisual[,] candies, GridManager grid, int rowA, int colA, int rowB, int colB, CandyVisual candyA, CandyVisual candyB)
    {
        candies[rowA, colA] = candyB;
        candies[rowB, colB] = candyA;

        Vector2 posA = GetWorldPosition(rowA, colA);
        Vector2 posB =GetWorldPosition(rowB, colB);

        Vector3 candyPosA =  grid.transform.position + new Vector3(posA.x, posA.y, -1);
        Vector3 candyPosB = grid.transform.position + new Vector3(posB.x, posB.y, -1);


        candyA.GetComponent<CandyVisual>().SetPositionCandy(candyPosA);
        candyB.GetComponent<CandyVisual>().SetPositionCandy(candyPosB);

        candyA.GetComponent<CandyVisual>().SetPositionGrid(rowA, colA);
        candyB.GetComponent<CandyVisual>().SetPositionGrid(rowB, colB);

    }

}
