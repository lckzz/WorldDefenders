using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNode : MonoBehaviour
{
    [SerializeField] private Define.SubStage stage;
    private List<Define.MonsterType> stageMonsterList = new List<Define.MonsterType>();

    public Define.SubStage Stage { get { return stage; } }      //������ ����������ġ�� ������
    public List<Define.MonsterType> StageMonsterList { get { return stageMonsterList; } }

    private Define.StageState stageStage = Define.StageState.Lock;
    public Define.StageState StState { get { return stageStage; } }      //������ �����������¸� ������

    [SerializeField] private Image stageImg;
    [SerializeField] private Image stageSubImg;
    [SerializeField] private Image stageTxtImg;
    [SerializeField] private Image lockImg;




    private Color stageLockColor = new Color32(82, 82, 82,255);
    private Color stageNormalColor = new Color32(255, 255, 255, 255);
    private Color stageOpenColor = new Color32(155, 245, 112, 255);
    private Color bossstageOpneColor = new Color32(255, 117, 125, 255);


    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    // Update is called once per frame

    
    void Init()
    {
        //�ش� ���������� ���� ���͸���Ʈ�� �������ְ� ���� ���������� ���¸� ������
        switch (stage)
        {
            case Define.SubStage.One:
                for(int ii = 0;ii < (int)Define.MonsterType.BowSkeleton + 1;ii++)
                {
                    stageMonsterList.Add((Define.MonsterType)ii);
                }

                stageStage = Define.StageState.Open;
                break;
            case Define.SubStage.Two:
                for (int ii = 0; ii < (int)Define.MonsterType.BowSkeleton + 1; ii++)
                {
                    stageMonsterList.Add((Define.MonsterType)ii);
                }
                stageStage = Define.StageState.Lock;

                break;
            case Define.SubStage.Three:
                for (int ii = 0; ii < (int)Define.MonsterType.BowSkeleton + 1; ii++)
                {
                    stageMonsterList.Add((Define.MonsterType)ii);
                }
                stageStage = Define.StageState.Lock;

                break;
            case Define.SubStage.Boss:
                for (int ii = 0; ii < (int)Define.MonsterType.BowSkeleton + 1; ii++)
                {
                    stageMonsterList.Add((Define.MonsterType)ii);
                }
                stageStage = Define.StageState.Lock;

                break;

        }



        if(stageStage == Define.StageState.Lock)
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

        else if(stageStage == Define.StageState.Open)
        {
            //���� ����
            if (stageImg != null)
            {
                if (lockImg.gameObject.activeSelf)
                    lockImg.gameObject.SetActive(false);

                if(stage != Define.SubStage.Boss)
                {
                    stageImg.color = stageOpenColor;
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
}
