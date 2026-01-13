using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTween : MonoBehaviour
{
    //[SerializeField] private RectTransform[] buttonRTF;
    public Ease ease=Ease.Linear;
    public float time=1;

    // Start is called before the first frame update
    private void OnEnable()
    {
        this.transform.localScale=Vector3.zero;
        this.transform.DOKill();

        this.transform.DOScale(Vector3.one,time).SetEase(ease).SetUpdate(true);
    }

    void Start()
    {
        // lỗi khi bật lại nó không thể tỉ lệ scale rỗ ràng bằng 0
    }

}

