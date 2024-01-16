using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DialogArrowPos : MonoBehaviour
{
    private RectTransform arrowRt;
    private float yPosPlus = 15.0f;
    private float yPosOld = .0f;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out arrowRt);
        yPosOld = arrowRt.anchoredPosition.y;
        RepeatArrow();
    }

    void RepeatArrow()
    {
        arrowRt.DOAnchorPosY(yPosOld + yPosPlus, 1).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        arrowRt.DOKill();
    }

    private void OnDisable()
    {
        arrowRt.DOKill();
    }

    private void OnEnable()
    {
        if(arrowRt != null)
         RepeatArrow();
    }


}
