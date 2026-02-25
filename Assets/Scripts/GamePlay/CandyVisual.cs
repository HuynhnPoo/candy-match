using UnityEngine;
using static CandyType;

public class CandyVisual : MonoBehaviour
{

    private float speed = 5f;
   
    private int row = 0;
    public int Row => row;
    private int colum = 0;

    public int Colum => colum;

    private GridManager grid;
    [SerializeField]
    private CandyTypeList type;

    private Vector3 targetPos;
    Vector3 startPos;
    Vector3 endPos;

    [SerializeField] private GameObject destroyEffectPT;

    int swipeDistance = 20;

    public CandyTypeList TypeCandy
    {
        get => type;
        set => type = value;
    }
    public void SetGridManager(GridManager grid)
    {
        this.grid = grid;
    }

    public void SetScale(float size)
    {
        //Debug.Log(thuwj)
        this.transform.GetChild(0).localScale = new Vector2(size, size);
    }

    public void SetPositionCandy(Vector3 pos)
    {
        this.targetPos = pos;
    }

    public void SetPositionGrid(int row, int colum)
    {
        this.row = row;
        this.colum = colum;
    }

    public void SetTypeCandy(CandyTypeList newType,bool isSameType)
    {

        if (isSameType && type == newType) // cung kiểu mới có thẻ câp nhật
        {
             type = newType;
            this.gameObject.name = newType.ToString();
            CandyName.LoadName(newType.ToString(), this);
        }

        else
        {
            type = newType;
            this.gameObject.name = newType.ToString();
            CandyName.LoadName(newType.ToString(), this);
        }



    }

    private void OnEnable()
    {
        CandyName.LoadName(this.transform.name.Replace("(Clone)", ""), this);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position != targetPos)
        {
            transform.position = Vector3.Lerp(this.transform.position, targetPos, this.speed * Time.deltaTime);
            if (Vector3.Distance(this.transform.position, targetPos) < 0.01f)
            {
                this.transform.position = targetPos;

            }
        }

    }

    private void OnMouseDown()
    {
        startPos = Input.mousePosition; //lấy tọa độ đàu tiên khi click chuột
       
        if (GameManager.Instance.IsClearCandy) // laasy tọa độ khi boost thực hiện
        {
            grid.ActiveClearRow(this.row);
            GameManager.Instance.IsClearCandy = false;
        }
    }

    private void OnMouseUp()
    {
        if (!GameManager.Instance.IsClearCandy)
        {
            endPos = Input.mousePosition;
            Vector2 swipe = endPos - startPos;

            if (swipe.magnitude < swipeDistance)
            {
                Debug.Log("hien ra "+ this.row +" " +$"{ this.colum}");
                this.grid.SelectCandy(this.row, this.colum);

            }
            else
            {
                DetectSwipeDirection(swipe);
            }
          
        }
        
    }

    void DetectSwipeDirection(Vector2 swipe)
    {
        swipe.Normalize();
        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            if (swipe.x > 0)
            {
                grid.SwipeCandy(row, colum, Vector2Int.right);
            }
            else
            {
                grid.SwipeCandy(row, colum, Vector2Int.left);
            }
        }
        else
        {
            if (swipe.y > 0)
            {
                grid.SwipeCandy(row, colum, Vector2Int.up);
            }
            else
            {
                grid.SwipeCandy(row, colum, Vector2Int.down);
            }
        }
    }

    public void DestroyCandy()// sinh effect
    {

        GameObject obj = Instantiate(destroyEffectPT, this.targetPos, Quaternion.identity);
        Destroy(obj, 1f);
    }

}
