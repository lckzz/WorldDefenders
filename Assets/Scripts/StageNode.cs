using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageNode : MonoBehaviour
{
    [SerializeField] private Define.Stage stage;
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

    private const int MONSTERLIST_TOTAL_COUNT = 5;


    private Color stageLockColor = new Color32(82, 82, 82, 255);
    private Color stageNormalColor = new Color32(255, 255, 255, 255);
    private Color stageNonClickColor = new Color32(176, 0, 255, 255);       //보라색
    private Color stageClickColor = new Color32(203, 95, 53, 255);         //자홍색

    private Color bossstageOpneColor = new Color32(255, 0, 0, 255);


    private List<int> stageMonsterIntList = new List<int>();
    [SerializeField] private List<Define.MonsterType> stageMonsterList = new List<Define.MonsterType>();

    private Dictionary<int, int> stageMonsterDict;

    public Define.Stage Stage { get { return stage; } }      //현재의 스테이지위치를 보내줌
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

        stageMonsterDict = new Dictionary<int, int>
        {
            { 0,Managers.Game.OneChapterStageInfoList[(int)stage].StageData.appearMonster1 },
            { 1,Managers.Game.OneChapterStageInfoList[(int)stage].StageData.appearMonster2 },
            { 2,Managers.Game.OneChapterStageInfoList[(int)stage].StageData.appearMonster3 },
            { 3,Managers.Game.OneChapterStageInfoList[(int)stage].StageData.appearMonster4 },
            { 4,Managers.Game.OneChapterStageInfoList[(int)stage].StageData.appearMonster5 },

        };



        StageMonsterSetting();

        if((int)stage > 0 && Managers.Game.OneChapterStageInfoList[(int)stage].state == 0)      //첫스테이지가 아니면서 이미 잠금상태라면
        {
            if (Managers.Game.OneChapterStageInfoList[(int)stage - 1].clear == true)        //전단계가 클리어라면
                Managers.Game.OneChapterStageInfoList[(int)stage].state = 1;        //열림 상태로

        }


        stageState = StageEnumToInt<Define.StageState>(Managers.Game.OneChapterStageInfoList[(int)stage].state);



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

                if(stage != Define.Stage.Boss)
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


    T StageEnumToInt<T>(int idx) where T : System.Enum
    {
        return (T)System.Enum.ToObject(typeof(T),idx);
    }


    private void StageMonsterSetting()
    {
        foreach(int idx in GetStageMonsterDict())
        {
            //딕셔너리를 돌아서
            if(idx >= 0)  //0보다 같거나 크면
                stageMonsterList.Add(StageEnumToInt<Define.MonsterType>(idx));      //스테이지 몬스터리스트에 넣어주기

            
        }

            
        //stageMonsterList.Add(StageEnumToInt<Define.MonsterType>(Managers.Game.OneChapterStageInfoList[(int)stage].StageData.appearMonster1));
        //stageMonsterList.Add(StageEnumToInt<Define.MonsterType>(Managers.Game.OneChapterStageInfoList[(int)stage].StageData.appearMonster2));
        //stageMonsterList.Add(StageEnumToInt<Define.MonsterType>(Managers.Game.OneChapterStageInfoList[(int)stage].StageData.appearMonster3));
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
        stageImg.color = (stage == Define.Stage.Boss) ? bossstageOpneColor : stageImg.color = stageNonClickColor;
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



    private IEnumerable GetStageMonsterDict()
    {
        return stageMonsterDict.Values;
    }
}
