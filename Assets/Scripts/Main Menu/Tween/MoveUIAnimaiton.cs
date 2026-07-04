using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveUIAnimaiton : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    public Ease ease = Ease.Linear;
    public float time = 1;
    int tempPos;

    [SerializeField]float distance ;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

       tempPos=(int)rectTransform.anchoredPosition.x;
    }
    private void OnEnable()
    {
        Camera mainCamera = Camera.main;
        Vector3 Edge=mainCamera.ViewportToScreenPoint(new Vector3(distance,this.rectTransform.anchoredPosition.y,mainCamera.nearClipPlane));

        rectTransform.anchoredPosition = new Vector3(Edge.x, this.rectTransform.anchoredPosition.y,0);
       rectTransform.DOAnchorPosX(tempPos, time).SetEase(ease);
    }


}
