using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageNameTxt;
    [SerializeField] private TextMeshProUGUI stageLvTxt;

    [SerializeField] private Image stageMonsterImg1;
    [SerializeField] private Image stageMonsterImg2;
    [SerializeField] private Image stageMonsterImg3;

    private RectTransform rt;

    private readonly string[] stageLvStr = { "1 ~ 3", "4 ~ 7", "8 ~ 10" };


    private readonly string[] stageNameStr = { "¼­ºÎ ½£Áö´ë", "µ¿ºÎ ½£Áö´ë", "³²ºÎ ½£Áö´ë" };



    [SerializeField] private Sprite[] meleeSkeletons;
    [SerializeField] private Sprite[] bowSkeletons;


    private void OnEnable()
    {


        StageInfoInit();

    }


    public void StageInfoInit()
    {
        if (rt == null)
            TryGetComponent(out rt);

        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 10);
        rt.DOSizeDelta(new Vector2(rt.sizeDelta.x, 294.0f), 0.25f).SetEase(Ease.OutQuad);

        stageMonsterImg1.sprite = meleeSkeletons[(int)Managers.Game.CurStageType];
        stageMonsterImg2.sprite = bowSkeletons[(int)Managers.Game.CurStageType];
        stageNameTxt.text = stageNameStr[(int)Managers.Game.CurStageType];
        stageLvTxt.text = stageLvStr[(int)Managers.Game.CurStageType];
    }

    public void SetStageInfoPosition(Vector3 pos)
    {
        Vector3 stagePos = rt.localPosition;
        stagePos.x = pos.x + 230.0f;
        stagePos.y = pos.y;
        rt.localPosition = stagePos;
    }


}
