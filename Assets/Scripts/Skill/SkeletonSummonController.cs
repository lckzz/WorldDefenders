using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using static Define;

public class SkeletonSummonController : SkillBase
{
    //��Ż�� �����ǰ� ������ ��Ż���� ���̷����� �����Ѵ�.

    
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

        //���� ������� �����յ� ������� �ؾߴ�
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


        yield return wfs;   //�ð��� ����� ��ȯ


        for (int ii = 0; ii < summonMonList.Count; ii++)
        {
            summonObj = Managers.Resource.Instantiate(summonMonList[ii], this.transform.position);          //���ӿ�����Ʈ�� �������ְ�
            summonObj.TryGetComponent(out eliteMonsterController);
            eliteMonsterController.IsSummon = true;                 //��ȯ�� ����Ʈ���ʹ� ����� �˹��� ����Ű�� �ʴ´�.
            yield return wfs;   //�ð��� ����� ��ȯ

        }


        if (anim != null)
        {
            anim.SetTrigger("Destroy");
        }

    }

}
