using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
public abstract class InputBase : MonoBehaviour, ICompoment, IPointerClickHandler
{
    [SerializeField] protected TMP_InputField input;
    [SerializeField] protected bool isPassword = false; // Gán trong Inspector nếu là password
    [SerializeField] protected string inputId; // ID duy nhất cho mỗi ô input

    public void LoadCompoment()
    {
        if (input == null) input = GetComponent<TMP_InputField>();
    }

    private void Awake()
    {
        this.LoadCompoment();

        // Nếu có inputId thì ép GameObject này mang tên đó,
        // để JS có thể gọi SendMessage(inputId, ...)
        if (!string.IsNullOrEmpty(inputId))
        {
            gameObject.name = inputId;
        }
    }

    private void Start()
    {
        this.AddEventListener();

        // Khi chọn vào InputField
        // input.onSelect.AddListener(OnInputFieldSelected);
    }

    /* private void OnInputFieldSelected(string currentText)
     {
 #if UNITY_WEBGL && !UNITY_EDITOR
         if (DeviceDetector.IsMobilePlatformInWebGL())
         {
             // Gọi JS để hiện bàn phím ảo
             FocusInputField(gameObject.name, isPassword);
         }
 #endif
     }*/

    protected virtual void AddEventListener()
    {
        this.input.onEndEdit.AddListener(this.OnEndEdit);
    }

    // Abstract: class con phải implement
    protected abstract void OnEndEdit(string text);

    // ========== Các hàm Unity nhận từ JS ==========

    // Nhận khi user nhập text
    public void OnKeyboardValueChanged(string val)
    {
        if (input == null) return;

        input.text = val;
        input.caretPosition = input.text.Length;
        input.ForceLabelUpdate();
    }

    // Nhận khi user nhấn Enter trên bàn phím ảo
    public void OnEndEditFromJs(string text)
    {
        input.text = text;
        this.OnEndEdit(text);

#if UNITY_WEBGL && !UNITY_EDITOR
        HideInputField();
#endif
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (input != null && !input.isFocused) input.Select();
#if UNITY_WEBGL && !UNITY_EDITOR
        if (DeviceDetector.IsMobilePlatformInWebGL())
        {
            // BỎ QUA onSelect, GỌI THẲNG JS Ở ĐÂY
            FocusInputField(gameObject.name, isPassword);
        }
#endif

    }

    // ========== JS Interop ==========

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void FocusInputField(string unityObjName, bool isPassword);
    [DllImport("__Internal")] private static extern void HideInputField();
#endif

}
