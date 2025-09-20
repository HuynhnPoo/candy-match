using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour, ICompoment
{
    private int height = 4;
    public int Height => height;

    private int width = 4;
    public int Width => width;

    private int cellSize = 1;
    public int CellSize => cellSize;
    private float spacing = 0.1f;
    public float Spacing => spacing;

    [SerializeField] private GameObject[] candyPrefabs;

    public Board board { private set; get; }


    private CandyVisual[,] visualGrid;
    [SerializeField] private GameObject backgroundPrefabs;

    private Vector2Int? selectCandy = null;

    public void LoadCompoment()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        board = new Board(this.width, this.height, this.cellSize, this.spacing);
        visualGrid = new CandyVisual[this.width, this.height];
        InstantiateGird();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void InstantiateGird()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                Vector2 pos2D = board.GetWorldPosition(x, y);
                Vector3 gird = this.transform.position + new Vector3(pos2D.x, pos2D.y, 0);


                GameObject obj = Instantiate(backgroundPrefabs, gird, Quaternion.identity, this.transform);

                int candy = board.GetCandy(x, y);
                Vector3 candyPos = this.transform.position + new Vector3(pos2D.x, pos2D.y, -1);
                Vector3 candyPosStart = this.transform.position + new Vector3(pos2D.x, this.height * (this.cellSize + this.spacing), -1);
                GameObject newCandy = Instantiate(candyPrefabs[candy], candyPosStart, Quaternion.identity, this.transform);
                //Debug.Log("debug gia tra cua can laf gif "+candy);

                CandyVisual candyVisual = newCandy.GetComponent<CandyVisual>();
                if (candyVisual == null) return;
                visualGrid[x, y] = candyVisual;
                candyVisual.SetPositionGrid(x, y);
                candyVisual.SetPositionCandy(candyPos);

                candyVisual.SetGridManager(this);
            }
        }
    }

    public void SelectCandy(int x, int y)
    {
        if (selectCandy == null)
        {
            selectCandy = new Vector2Int(x, y);
        }
        else
        {
            Vector2Int first = selectCandy.Value;

            if (Mathf.Abs(first.x - x) == 1 && first.y == y ||
                Mathf.Abs(first.y - y) == 1 && first.x == x)
            {

                board.Swap(visualGrid, this, first.x, first.y, x, y);

                selectCandy = null;

            }
            else
            {
                selectCandy = null;
            }
        }
    }
    public bool CheckMatchesForSwap(int rowA, int colA, int rowB, int colB)
    {
        HashSet<CandyVisual> matchesA = MatchCandy.FindAllMacth(visualGrid, width, height, rowA, colA);
        HashSet<CandyVisual> matchesB = MatchCandy.FindAllMacth(visualGrid, width, height, rowB, colB);

        HashSet<CandyVisual> allMatches = new HashSet<CandyVisual>();
        allMatches.UnionWith(matchesA);
        allMatches.UnionWith(matchesB);
        if (allMatches.Count >= 3)
        {
          MatchCandy.DestroyAnfndRefill(visualGrid,this,new List<CandyVisual>(allMatches),candyPrefabs);
            return true;
        }
        return false;
    }



}