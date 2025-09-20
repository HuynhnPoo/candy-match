using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonBase : MonoBehaviour,ICompoment
{
    protected Button button;

    public void LoadCompoment()
    {
        if (this.button == null) 
       this.button = GetComponent<Button>();
    }

    protected virtual void Awake()
    {
        this.LoadCompoment();
    }
    protected virtual void OnEnable()
    {
    }

    private void Start()
    {
        this.AddEventListener();
        
    }
    public virtual void AddEventListener()
    {
        button.onClick.AddListener(this.OnClick);
    }
    public abstract void OnClick();

}
