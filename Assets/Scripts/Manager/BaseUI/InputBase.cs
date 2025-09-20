using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class InputBase : MonoBehaviour,ICompoment
{

    [SerializeField] protected TMP_InputField input;

    public void LoadCompoment()
    {
        if (input == null) input = GetComponent<TMP_InputField>();
    }
    private void Awake()
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

    protected virtual void AddEventListener()
    {
        this.input.onEndEdit.AddListener(this.OnEndEdit);
    }

   protected abstract void OnEndEdit(string text);

}
