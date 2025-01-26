using System;
using DG.Tweening;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] private float _yMovement;
    [SerializeField] private float _xMovement;
    [SerializeField] private float _yDuration;
    [SerializeField] private float _xDuration;
    private void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        
        rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + _yMovement, _yDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + _xMovement, _xDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}