using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNode : MonoBehaviour
{
    [SerializeField] private Define.SubStage stage;
    private List<Define.MonsterType> stageMonsterList = new List<Define.MonsterType>();

    public Define.SubStage Stage { get { return stage; } }      //현재의 스테이지위치를 보내줌
    public List<Define.MonsterType> StageMonsterList { get { return stageMonsterList; } }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame

    
    void Init()
    {
        //해당 스테이지에 따라서 몬스터리스트를 갱신해준다.
        switch (stage)
        {
            case Define.SubStage.One:
                for(int ii = 0;ii < (int)Define.MonsterType.BowSkeleton + 1;ii++)
                {
                    stageMonsterList.Add((Define.MonsterType)ii);
                }
                break;
        }

    }
}
