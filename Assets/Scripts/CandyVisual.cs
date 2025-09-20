using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CandyType;

public class CandyVisual : MonoBehaviour
{

    private Vector3 targetPos;
    private float speed = 5f;
    private int row = 0;
    public int Row => row;
    private int colum = 0;

    public int Colum => colum;

    private GridManager grid;

    private CandyTypeList type;

    public CandyTypeList TypeCandy
    {
        get => type;
        set => type = value;
    }
   public void SetGridManager(GridManager grid) 
    {
        this.grid = grid;
    }

    void SetScale()
    {
        this.transform.GetChild(0).localScale = new Vector2(0.5f, 0.5f);
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

    private void OnEnable()
    {
         CandyName.LoadName(this.transform.name.Replace("(Clone)",""),this);
    }
    // Start is called before the first frame update
    void Start()
    {
        SetScale();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position !=targetPos)
        {
           transform.position = Vector3.Lerp(this.transform.position,targetPos,this.speed*Time.deltaTime);
            if (Vector3.Distance(this.transform.position, targetPos) < 0.01f)
            {
                this.transform.position = targetPos;

            }
        }


    }

    private void OnMouseDown()
    {
        this.grid.SelectCandy(this.row,this.colum);

        Debug.Log("ten hang " + row +"ten cot"+colum);
    }

  
}

