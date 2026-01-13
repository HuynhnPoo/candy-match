using UnityEngine;
using UnityEngine.UI;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
public abstract class ButtonBase : MonoBehaviour, ICompoment
{
    /*#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void HideInputField();
    #endif*/

    protected Button button;

    public void LoadCompoment()
    {
        if (this.button == null)
            this.button = GetComponent<Button>();
    }

    protected virtual void Awake()
    {
    }
    protected virtual void OnEnable()
    {
        this.LoadCompoment();
    }

    protected virtual void Start()
    {
     
        this.AddEventListener();

    }
    public virtual void AddEventListener()
    {

        this.button.onClick.AddListener(() => {
            SoundManager.Instance.PlaySfx("Click_Button");
            this.OnClick(); });
    }
    public abstract void OnClick();

    /* protected virtual void CloseKeyboard()
     {
 #if UNITY_WEBGL && !UNITY_EDITOR
         HideInputField();
 #endif
     }*/

}
