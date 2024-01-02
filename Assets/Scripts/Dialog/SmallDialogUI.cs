using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDialogUI : BaseDialogueUI
{

    [SerializeField] private RectTransform rt;
    private GameObject dialogGo;

    Vector2 oldPos = Vector2.zero;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        dialogGo = transform.GetChild(0).gameObject;
        dialogGo?.TryGetComponent(out rt);

        oldPos = rt.anchoredPosition;
    }

    public void DialogPositionChange(float yPos)  //소 다이얼로그의 위치 변환
    {
        Vector2 pos = rt.anchoredPosition;
        pos.y = yPos;
        rt.anchoredPosition = pos;
    }
    public void DialogPositionReset()
    {
        rt.anchoredPosition = oldPos;
    }


}
