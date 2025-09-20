using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TextBase : MonoBehaviour,ICompoment
{
    [SerializeField] protected Text text;

    public void LoadCompoment()
    {
        if (text == null)
        this.text=GetComponent<Text>();
    }

    protected virtual void Awake()
    { 
        this.LoadCompoment();

    }
    protected virtual void Update()
    {
        this.PrintText();
    }
    protected abstract void PrintText();
    
}
