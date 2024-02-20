using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using static Define;

public class SkeletonSummonController : SkillBase
{
    //포탈이 생성되고 생성된 포탈에서 스켈레톤을 생성한다.

    
    private Unit owener;
    private Vector3 pos;
    private Vector3 curPos;

    private Animator anim;
    //int summonidx;
    private bool firstSummon = false;

    private EliteMonsterController eliteMonsterController;
    private GameObject summonObj;


    private List<GameObject> summonMonList;
    private MonsterType[] monTypes = { MonsterType.EliteWarrior, MonsterType.EliteShaman, MonsterType.EliteCavalry };



    public void SetInfo(Unit owner, SkillData data, Vector3 startPos)
    {
        //if (Managers.Data.magicSkillDict.TryGetValue(Unitlv, out SkillData data) == false)
        //{
        //    Debug.LogError("ProjecteController SetInfo Failed");
        //    return;
        //}

        owener = owner;
        pos = startPos;
        SkillData = data;
        this.transform.position = pos;
        // TODO : Data Parsing
    }

    public override bool Init()
    {
        base.Init();
        TryGetComponent(out anim);
        summonMonList = new List<GameObject>();

        foreach (MonsterType monType in monTypes)
        {
            GameObject prefabObj = Managers.Resource.Load<GameObject>($"Prefabs/{Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[monType]].monsterPrefab}");
            summonMonList.Add(prefabObj);
        }
        return true;
    }

    public override void UpdateController()
    {
        base.UpdateController();

        //적이 사라지면 프리팹도 사라지게 해야댐
        //if (owener == null || enemy.IsDie == true)
        //    Destroy(this.gameObject);

    }


    public void StartSummon()
    {
        if(firstSummon == false)
        {
            firstSummon = true;
            StartCoroutine(SkeletonSummon());

        }
    }

    WaitForSeconds wfs = new WaitForSeconds(1.0f);
    IEnumerator SkeletonSummon()
    {


        yield return wfs;   //시간초 대기후 소환


        for (int ii = 0; ii < summonMonList.Count; ii++)
        {
            summonObj = Managers.Resource.Instantiate(summonMonList[ii], this.transform.position);          //게임오브젝트를 생성해주고
            summonObj.TryGetComponent(out eliteMonsterController);
            eliteMonsterController.IsSummon = true;                 //소환된 엘리트몬스터는 등장시 넉백을 일으키지 않는다.
            yield return wfs;   //시간초 대기후 소환

        }


        if (anim != null)
        {
            anim.SetTrigger("Destroy");
        }

    }

}
