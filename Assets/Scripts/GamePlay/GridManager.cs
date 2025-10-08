using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour, ICompoment
{
    [SerializeField] private LevelDatabase levelDatabase;
    private LevelLayoutData currentLevelLayout;


    private int height = 4;
    public int Height => height;

    private int width = 4;
    public int Width => width;

    private int cellSize = 1;
    public int CellSize => cellSize;
    private float spacing = 0.1f;
    public float Spacing => spacing;

    [SerializeField] private GameObject[] candyPrefabs;
     private GameObject[,] selectVisualGrid;
    [SerializeField] private GameObject backgroundPrefabs;
    [SerializeField] private GameObject selectPrefabs;
    [SerializeField] private GameObject blockedCellPrefab;

    private CandyVisual[,] visualGrid;

    public Board board { private set; get; }
    private bool[,] levelLayout;



    private Vector2Int? selectCandy = null;



    void LoadCurrentLevel()
    {
        if (levelDatabase == null)
        {
            Debug.LogError("Level Database not assigned!");
            CreateDefaultLayout();
            return;
        }

        //  int currentLevel = GameManager.Instance.CurrentLevel;
        currentLevelLayout = levelDatabase.GetLevelLayout(1);
        Debug.Log(levelDatabase + "tdfg" + currentLevelLayout);

        if (currentLevelLayout == null)
        {
            CreateDefaultLayout();
            return;
        }

        width = currentLevelLayout.width;
        height = currentLevelLayout.height;
        Debug.Log($"hien thi rong {width} cao{height}  ");


        levelLayout = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                levelLayout[x, y] = currentLevelLayout.GetCell(x, y);
            }
        }

    }

    void CreateDefaultLayout()
    {
        levelLayout = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                levelLayout[x, y] = true;
            }
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.Init();
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
        InstantiateGird();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) GameManager.Instance.Score = GameMechanics.AddScore(5);
        if (Input.GetKeyDown(KeyCode.L)) ReLoadLevel();

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

                GameObject selectObj = Instantiate(selectPrefabs, gird, Quaternion.identity, this.transform);

                CandyVisual candyVisual = newCandy.GetComponent<CandyVisual>();
                if (candyVisual == null) return;
                visualGrid[x, y] = candyVisual;
                selectVisualGrid[x, y] = selectObj;
                selectVisualGrid[x,y].SetActive(false);
                candyVisual.SetPositionGrid(x, y);
                candyVisual.SetPositionCandy(candyPos);

                candyVisual.SetGridManager(this);

            }
        }
    }

    void InstantiateGird2()
    {
        Debug.Log("🎮 Starting InstantiateGird...");

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos2D = board.GetWorldPosition(x, y);
                Vector3 gridPos = this.transform.position + new Vector3(pos2D.x, pos2D.y, 0);

                // ✅ KIỂM TRA LAYOUT - ĐÂY LÀ PHẦN QUAN TRỌNG!
                if (levelLayout != null && levelLayout[x, y] == true)
                {
                    // ✅ Ô này ĐƯỢC PHÉP spawn candy

                    // Tạo background
                    GameObject obj = Instantiate(backgroundPrefabs, gridPos, Quaternion.identity, this.transform);

                    // Apply màu nếu có
                    if (currentLevelLayout != null)
                    {
                        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                        if (sr != null)
                        {
                            sr.color = currentLevelLayout.levelColor;
                        }
                    }

                    // Tạo candy
                    int candy = board.GetCandy(x, y);
                    Vector3 candyPos = this.transform.position + new Vector3(pos2D.x, pos2D.y, -1);
                    Vector3 candyPosStart = this.transform.position + new Vector3(pos2D.x, this.height * (this.cellSize + this.spacing), -1);
                    GameObject newCandy = Instantiate(candyPrefabs[candy], candyPosStart, Quaternion.identity, this.transform);

                    CandyVisual candyVisual = newCandy.GetComponent<CandyVisual>();
                    if (candyVisual == null)
                    {
                        Debug.LogError($"❌ CandyVisual component not found at ({x}, {y})!");
                        continue;
                    }

                    visualGrid[x, y] = candyVisual;
                    candyVisual.SetPositionGrid(x, y);
                    candyVisual.SetPositionCandy(candyPos);
                    candyVisual.SetGridManager(this);

                    Debug.Log($"✅ Spawned candy at ({x}, {y})");
                }
                else
                {
                    // 🚫 Ô này BỊ KHÓA - KHÔNG spawn candy

                    // Tạo ô bị khóa (nếu có prefab)
                    if (blockedCellPrefab != null)
                    {
                        GameObject blockedCell = Instantiate(blockedCellPrefab, gridPos, Quaternion.identity, this.transform);
                        Debug.Log($"🚫 Created blocked cell at ({x}, {y})");
                    }
                    else
                    {
                        Debug.Log($"🚫 Skipped cell at ({x}, {y}) - No blocked prefab");
                    }

                    // Set visualGrid = null cho ô bị khóa
                    visualGrid[x, y] = null;
                }
            }
        }

        Debug.Log("✅ InstantiateGird completed!");
    }

    public void ReLoadLevel()
    {
        ClearGrid();

        LoadCurrentLevel();
        board = new Board(this.width, this.height, this.cellSize, this.spacing);
        visualGrid = new CandyVisual[this.width, this.height];

        InstantiateGird2();
    }

    // xoa tat car cac con 
    void ClearGrid()
    {
        for (int i = transform.childCount - 1; i > 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        //visualGrid = null;
        selectCandy = null;
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
    
    public void SelectCandy(int x, int y)
    {
        if (selectCandy == null)
        {
            selectCandy = new Vector2Int(x, y);
            selectVisualGrid[x,y].SetActive(true);
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

            }
            else
            {
                selectCandy = null;
                selectVisualGrid[x, y].SetActive(true);
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
            MatchCandy.DestroyAnfndRefill(visualGrid, this, new List<CandyVisual>(allMatches), candyPrefabs);
            // GameManager.Instance.Score = GameMechanics.AddScore(5);
            return true;
        }
        return false;
    }



}