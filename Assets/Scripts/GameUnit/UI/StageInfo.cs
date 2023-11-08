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
    [SerializeField] private TextMeshProUGUI stageClearGoldTxt;
    [SerializeField] private TextMeshProUGUI stageBestTimeTxt;



    [SerializeField] private Image stageMonsterImg1;
    [SerializeField] private Image stageMonsterImg2;
    [SerializeField] private Image stageMonsterImg3;


    private int sec = 0;
    private int min = 0;
    private RectTransform rt;

    private readonly string[] stageLvStr = { "1 ~ 3", "4 ~ 7", "8 ~ 10" };


    private readonly string[] stageNameStr = { "서부 숲지대", "동부 숲지대", "남부 숲지대" };



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
        
        switch (Managers.Game.CurStageType)
        {
            case Define.SubStage.West:
                stageClearGoldTxt.text = Managers.Game.WestStageGold.ToString();
                TimeScore(Managers.Game.WestStageBestTime);
                break;
            case Define.SubStage.East:
                stageClearGoldTxt.text = Managers.Game.EastStageGold.ToString();
                TimeScore(Managers.Game.EastStageBestTime);
                break;
            case Define.SubStage.South:
                stageClearGoldTxt.text = Managers.Game.SouthStageGold.ToString();
                TimeScore(Managers.Game.SouthStageBestTime);
                break;
        }


    }

    public void SetStageInfoPosition(Vector3 pos)
    {
        Vector3 stagePos = rt.localPosition;
        stagePos.x = pos.x + 230.0f;

        if (Managers.Game.CurStageType == Define.SubStage.West)
            stagePos.y = pos.y + 40.0f;
        else
         stagePos.y = pos.y + 10.0f;
        rt.localPosition = stagePos;
    }


    private void TimeScore(float bestTime)
    {

        if (bestTime <= 0.0f)
        {
            stageBestTimeTxt.text = "-- : --";
            return;
        }

        min = (int)bestTime / 60;
        sec = (int)bestTime % 60;

        TimeSet(min, sec);
    }

    private void TimeSet(int min, int sec)
    {
        if (min < 10 && sec < 10)
            stageBestTimeTxt.text = "0" + min.ToString() + " : " + "0" + sec.ToString();

        else if (min >= 10 && sec < 10)       //초만 10초 미만일때
            stageBestTimeTxt.text = min.ToString() + " : " + "0" + sec.ToString();

        else if (min < 10 && sec >= 10)       //분만 10초 미만일때
            stageBestTimeTxt.text = "0" + min.ToString() + " : " + sec.ToString();

        else if (min < 10 && sec < 10)       //초 분이 10 미만일때
            stageBestTimeTxt.text = "0" + min.ToString() + " : " + "0" + sec.ToString();

        else if (min >= 10 && sec < 10)       //초가 10 미만일때
            stageBestTimeTxt.text = min.ToString() + " : " + "0" + sec.ToString();

        else if (min < 10 && sec >= 10)       //분가 10 미만일때
            stageBestTimeTxt.text = "0" + min.ToString() + " : " + sec.ToString();

        else   //전부다 10 이상일때
            stageBestTimeTxt.text = min.ToString() + " : " + sec.ToString();
    }


}
