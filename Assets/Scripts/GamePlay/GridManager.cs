using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour, ICompoment
{

    private int height = 4;
    public int Height { set => height = value; get => height; }

    private int width = 4;
    public int Width { set => width = value; get => width; }

    private int cellSize = 1;
    public int CellSize { set => cellSize = value; get => cellSize; }

    private float spacing = 0.1f;
    public float Spacing { set => spacing = value; get => spacing; }
    private float localSize = 0.5f;
    public float LocalSize => localSize;

    private bool[,] levelLayout;
    public bool[,] LevelLayout => levelLayout;
    private Vector2Int? selectCandy = null;
    [SerializeField] private GameObject[] candyPrefabs;
    private GameObject[,] selectVisualGrid;

    [SerializeField] private GameObject backgroundPrefabs;
    [SerializeField] private GameObject selectPrefabs;
    [SerializeField] private GameObject blockedCellPrefab;

    public CandyVisual[,] visualGrid { set; get; }


    public Board board { set; get; }

    private LevelManager levelManager;


    private void OnEnable()
    {
        levelManager = GetComponent<LevelManager>();
        levelManager.SetGridManager(this);
    }
    public void LoadCompoment()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        board = new Board(this.width, this.height, this.cellSize, this.spacing);
        selectVisualGrid = new GameObject[this.width, this.height];
        visualGrid = new CandyVisual[this.width, this.height];

        GameManager.Instance.Init();

        if (GameManager.Instance.CurrentLevel == 0)
        {

            LoadAndInstantiateGrid();
        }
        else
        {
            this.cellSize = (int)0.4f; //
            this.spacing = 0.6f;// khaonrg cach
            this.localSize = 0.3f;// độ lớn
            this.transform.position = new Vector3(-2.15f, -3.15f, 0);// vị trí

            levelManager.LoadNewLevelData(GameManager.Instance.CurrentLevel);
            LoadAndInstantiateGrid();
        }
    }

    // Update is called once per frame
    void Update()
    {
         /* if (Input.GetKeyDown(KeyCode.Z))
        {

            this.cellSize = (int)0.4f;
            this.spacing = 0.6f;
            this.localSize = 0.3f;
            this.transform.position = new Vector3(-2.15f, -3.15f, 0);
            levelManager.LoadNewLevelData(1);
            LoadAndInstantiateGrid();
        } */
        
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            //
        } 
        
       

    }

    void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);
    }

    public void SetLevelLayout(bool[,] layout)
    {
        this.levelLayout = layout;
    }

    public void LoadAndInstantiateGrid()
    {
        ClearGrid();

        board = new Board(Width, Height, CellSize, Spacing);
        visualGrid = new CandyVisual[Width, Height];
        selectVisualGrid = new GameObject[Width, Height];

        if (levelLayout == null)
        {
            Debug.LogWarning("⚠️ Level layout is null, creating default layout.");
            levelLayout = new bool[Width, Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    levelLayout[x, y] = true;
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2 pos2D = board.GetWorldPosition(x, y);
                Vector3 gridPos = transform.position + new Vector3(pos2D.x, pos2D.y, 0);

                if (levelLayout[x, y])
                {
                    // Spawn background
                    GameObject bg = Instantiate(backgroundPrefabs, gridPos, Quaternion.identity, transform);
                    bg.transform.localScale = new Vector3((localSize + 0.4f) - 0.2f, (localSize + 0.4f) - 0.2f, 1);

                    // Spawn candy
                    int candy = board.GetCandy(x, y);
                    Vector3 candyPos = transform.position + new Vector3(pos2D.x, pos2D.y, -1);
                    GameObject newCandy = Instantiate(candyPrefabs[candy], candyPos, Quaternion.identity, transform);

                    CandyVisual candyVisual = newCandy.GetComponent<CandyVisual>();
                    visualGrid[x, y] = candyVisual;

                    // Spawn select visual
                    GameObject selectObj = Instantiate(selectPrefabs, gridPos, Quaternion.identity, transform);
                    selectObj.SetActive(false);
                    selectVisualGrid[x, y] = selectObj;

                    candyVisual.SetGridManager(this);
                    candyVisual.SetPositionGrid(x, y);
                    candyVisual.SetPositionCandy(candyPos);
                    candyVisual.SetScale(localSize);
                }
                else
                {
                    // Spawn blocked cell
                    if (blockedCellPrefab != null)
                    {
                        GameObject blocked = Instantiate(blockedCellPrefab, gridPos, Quaternion.identity, this.transform);
                        blocked.transform.localScale = new Vector3((localSize + 0.4f) - 0.2f, (localSize + 0.4f) - 0.2f, 1);
                    }
                }
            }
        }

        Debug.Log("✅ Grid instantiated successfully!");
    }

    void HidePreviousSelection()
    {
        if (selectCandy.HasValue)
        {
            int oldX = selectCandy.Value.x;
            int oldY = selectCandy.Value.y;

            // ⭐ ẨN đối tượng chọn cũ
            if (selectVisualGrid[oldX, oldY] != null)
            {
                selectVisualGrid[oldX, oldY].SetActive(false);
            }
        }
    }

    public void SelectCandy(int x, int y) // chọn candy
    {
        if (selectCandy == null)
        {
            selectCandy = new Vector2Int(x, y);
            selectVisualGrid[x, y].SetActive(true);
        }
        else
        {
            Vector2Int first = selectCandy.Value;
            HidePreviousSelection();

            if (Mathf.Abs(first.x - x) == 1 && first.y == y ||
                Mathf.Abs(first.y - y) == 1 && first.x == x)
            {

                board.Swap(visualGrid, this, first.x, first.y, x, y);

                selectCandy = null;
                GameManager.Instance.MoveStep--;
            }
            else if (first.x == x || first.y == y)
            {
                selectCandy = null;
                HidePreviousSelection();
                return;
            }
            else
            {
                //  selectCandy = null;
                selectCandy = new Vector2Int(x, y);
                selectVisualGrid[x, y].SetActive(true);
            }
        }
    }

    public void SwipeCandy(int row, int col, Vector2Int direction)
    {
        int newRow = row + direction.x;
        int newCol = col + direction.y;

        board.Swap(visualGrid, this, row, col, newRow, newCol);
        HidePreviousSelection(); 
        selectCandy = null;

        GameManager.Instance.MoveStep--;

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
            MatchCandy.DestroyAnfndRefill(visualGrid, this, new List<CandyVisual>(allMatches), candyPrefabs);
            // GameManager.Instance.Score = GameMechanics.AddScore(5);
            return true;
        }
        return false;
    }

    public void ActiveClearRow(int posCandy)
    {
        MatchCandy.ClearRow(visualGrid,this,posCandy,candyPrefabs);
    }
 }