using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    private RectTransform stageImgRt;
    private RectTransform stageSubImgRt;
    private RectTransform stageObjectiveRt;





    private Color stageLockColor = new Color32(82, 82, 82, 255);
    private Color stageNormalColor = new Color32(255, 255, 255, 255);
    private Color stageNonClickColor = new Color32(176, 0, 255, 255);       //�����
    private Color stageClickColor = new Color32(203, 95, 53, 255);         //��ȫ��

    private Color bossstageOpneColor = new Color32(255, 117, 125, 255);



    private List<Define.MonsterType> stageMonsterList = new List<Define.MonsterType>();

    public Define.SubStage Stage { get { return stage; } }      //������ ����������ġ�� ������
    public List<Define.MonsterType> StageMonsterList { get { return stageMonsterList; } }

    private Define.StageState stageState = Define.StageState.Lock;
    public Define.StageState StState { get { return stageState; } }      //������ �����������¸� ������



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

        //�ش� ���������� ���� ���͸���Ʈ�� �������ְ� ���� ���������� ���¸� ������
        switch (stage)
        {
            case Define.SubStage.West:
                for(int ii = 0;ii < (int)Define.MonsterType.BowSkeleton + 1;ii++)
                {
                    stageMonsterList.Add((Define.MonsterType)ii);
                }

                stageState = Define.StageState.Open;
                break;
            case Define.SubStage.East:
                for (int ii = 0; ii < (int)Define.MonsterType.BowSkeleton + 1; ii++)
                {
                    stageMonsterList.Add((Define.MonsterType)ii);
                }
                stageState = Define.StageState.Open;

                break;
            case Define.SubStage.South:
                for (int ii = 0; ii < (int)Define.MonsterType.BowSkeleton + 1; ii++)
                {
                    stageMonsterList.Add((Define.MonsterType)ii);
                }
                stageState = Define.StageState.Open;

                break;
            case Define.SubStage.Boss:
                for (int ii = 0; ii < (int)Define.MonsterType.BowSkeleton + 1; ii++)
                {
                    stageMonsterList.Add((Define.MonsterType)ii);
                }
                stageState = Define.StageState.Lock;

                break;

        }



        if(stageState == Define.StageState.Lock)
        {
            //���
            if(stageImg != null)
            {
                if (!lockImg.gameObject.activeSelf)
                    lockImg.gameObject.SetActive(true);

                stageImg.color = stageLockColor;
                stageSubImg.color = stageLockColor;
                stageTxtImg.color = stageLockColor;
            }
        }

        else if(stageState == Define.StageState.Open)
        {
            //���� ����
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
        stageImgRt?.DOSizeDelta(new Vector2(140.0f, 75.0f), 0.1f);
        stageSubImgRt?.DOSizeDelta(new Vector2(300.0f, 300.0f), 0.1f);
        stageObjectiveRt?.gameObject.SetActive(true);
        stageObjectiveRt?.DOSizeDelta(new Vector2(230.0f, stageObjectiveRt.sizeDelta.y), 0.3f);
        stageImg.color = stageClickColor;
    }

    public void ClickStageDoOff()
    {
        stageImgRt?.DOSizeDelta(new Vector2(100.0f, 50.0f), 0.1f);
        stageSubImgRt?.DOSizeDelta(new Vector2(200.0f, 200.0f), 0.1f);
        stageObjectiveRt?.DOSizeDelta(new Vector2(0.0f, stageObjectiveRt.sizeDelta.y), 0.3f);
        stageImg.color = stageNonClickColor;
    }
}
