using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageNode : MonoBehaviour
{
    [SerializeField] private Define.SubStage stage;
    [SerializeField] private Image stageImg;
    [SerializeField] private Image stageSubImg;
    [SerializeField] private Image stageTxtImg;
    [SerializeField] private Image lockImg;
    [SerializeField] private Image stageObjectiveImg;
    [SerializeField] private TextMeshProUGUI stageClearTxt;
    [SerializeField] private GameObject fireEffObj;


    private RectTransform stageImgRt;
    private RectTransform stageSubImgRt;
    private RectTransform stageObjectiveRt;

    private StageAnim stageAnim;




    private Color stageLockColor = new Color32(82, 82, 82, 255);
    private Color stageNormalColor = new Color32(255, 255, 255, 255);
    private Color stageNonClickColor = new Color32(176, 0, 255, 255);       //보라색
    private Color stageClickColor = new Color32(203, 95, 53, 255);         //자홍색

    private Color bossstageOpneColor = new Color32(255, 117, 125, 255);



    private List<Define.MonsterType> stageMonsterList = new List<Define.MonsterType>();

    public Define.SubStage Stage { get { return stage; } }      //현재의 스테이지위치를 보내줌
    public List<Define.MonsterType> StageMonsterList { get { return stageMonsterList; } }

    private Define.StageState stageState = Define.StageState.Lock;
    public Define.StageState StState { get { return stageState; } }      //현재의 스테이지상태를 보내줌



    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    // Update is called once per frame

    
    void Init()
    {
        if (stageImgRt == null)
            this.TryGetComponent(out stageImgRt);
        if (stageSubImgRt == null)
            stageSubImg?.TryGetComponent(out stageSubImgRt);
        if (stageObjectiveRt == null)
            stageObjectiveImg?.TryGetComponent(out stageObjectiveRt);

        if (stageAnim == null)
            this.gameObject.TryGetComponent(out stageAnim);




        //해당 스테이지에 따라서 몬스터리스트를 갱신해주고 현재 스테이지의 상태를 보여줌
        switch (stage)
        {
            case Define.SubStage.West:
                stageMonsterList.Add(Define.MonsterType.NormalSkeleton);
                stageMonsterList.Add(Define.MonsterType.BowSkeleton);
                stageMonsterList.Add(Define.MonsterType.EliteWarrior);

                stageState = Define.StageState.Open;
                
                //if(Managers.Game.WestStageClear)
                //{
                //    fireEffObj.SetActive(false);
                //    stageClearTxt.gameObject.SetActive(true);
                //}

                break;
            case Define.SubStage.East:
                stageMonsterList.Add(Define.MonsterType.MidSkeleton);
                stageMonsterList.Add(Define.MonsterType.MidBowSkeleton);
                stageMonsterList.Add(Define.MonsterType.EliteShaman);
                
                if(Managers.Game.WestStageClear)
                    stageState = Define.StageState.Open;
                else
                    stageState = Define.StageState.Lock;
                //if (Managers.Game.EastStageClear)
                //{
                //    fireEffObj.SetActive(false);
                //    stageClearTxt.gameObject.SetActive(true);
                //}

                break;
            case Define.SubStage.South:
                stageMonsterList.Add(Define.MonsterType.HighSkeleton);
                stageMonsterList.Add(Define.MonsterType.HighBowSkeleton);
                stageMonsterList.Add(Define.MonsterType.EliteCavalry);

                if (Managers.Game.EastStageClear)
                    stageState = Define.StageState.Open;
                else
                    stageState = Define.StageState.Lock;
                //if (Managers.Game.SouthStageClear)
                //{
                //    fireEffObj.SetActive(false);
                //    stageClearTxt.gameObject.SetActive(true);
                //}

                break;
        }



        if(stageState == Define.StageState.Lock)
        {
            //잠김
            if(stageImg != null)
            {
                if (!lockImg.gameObject.activeSelf)
                    lockImg.gameObject.SetActive(true);

                stageImg.color = stageLockColor;
                stageSubImg.color = stageLockColor;
                stageTxtImg.color = stageLockColor;
            }

            if (fireEffObj != null)
                fireEffObj.SetActive(false);
        }

        else if(stageState == Define.StageState.Open)
        {
            //들어갈수 있음
            if (stageImg != null)
            {
                if (lockImg.gameObject.activeSelf)
                    lockImg.gameObject.SetActive(false);

                if(stage != Define.SubStage.Boss)
                {
                    stageImg.color = stageNonClickColor;
                    stageSubImg.color = stageNormalColor;
                    stageTxtImg.color = stageNormalColor;
                }
                else
                {
                    stageImg.color = bossstageOpneColor;
                    stageSubImg.color = stageNormalColor;
                    stageTxtImg.color = stageNormalColor;
                }

            }
        }

    }

    public void ClickStageDoOn()
    {
        stageAnim?.StageAnimSet("Close", false);
        stageAnim?.StageAnimSet("Open",true);
        stageImgRt?.DOSizeDelta(new Vector2(140.0f, 75.0f), 0.1f);
        stageSubImgRt?.DOSizeDelta(new Vector2(130.0f, 130.0f), 0.1f);
        stageObjectiveRt?.gameObject.SetActive(true);
        stageObjectiveRt?.DOSizeDelta(new Vector2(230.0f, stageObjectiveRt.sizeDelta.y), 0.3f);
        stageImg.color = stageClickColor;

        if(stageClearTxt != null)
        {
            if (stageClearTxt.gameObject.activeSelf)
                stageClearTxt.DOFontSize(47.0f, 0.1f);
        }

    }

    public void ClickStageDoOff()
    {
        stageAnim?.StageAnimSet("Open", false);
        stageAnim?.StageAnimSet("Close", true);
        stageImgRt?.DOSizeDelta(new Vector2(100.0f, 50.0f), 0.1f);
        stageSubImgRt?.DOSizeDelta(new Vector2(102.0f, 107.0f), 0.1f);
        stageObjectiveRt?.DOSizeDelta(new Vector2(0.0f, stageObjectiveRt.sizeDelta.y), 0.3f);
        stageImg.color = stageNonClickColor;
        if (stageClearTxt != null)
        {
            if (stageClearTxt.gameObject.activeSelf)
                stageClearTxt.DOFontSize(37.0f, 0.1f);
        }

    }

    public Vector3 GetNodePosition()
    {
        return stageImgRt.localPosition;
    }
}
