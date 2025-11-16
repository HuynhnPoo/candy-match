using UnityEngine;
using UnityEngine.UI;

public abstract class SliderBase : MonoBehaviour
{
    [SerializeField] protected Slider slider;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (slider == null) slider = GetComponent<Slider>();
    }

    protected virtual void AddChangedEvent()
    {
        this.slider.onValueChanged.AddListener(this.OnChange);
    }

    protected abstract void OnChange(float amount);
}
