using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSummonController : SkillBase
{
    //��Ż�� �����ǰ� ������ ��Ż���� ���̷����� �����Ѵ�.

    
    Unit owener;
    Vector3 pos;
    Vector3 curPos;

    Animator anim;
    int summonidx;
    bool firstSummon = false;

    private List<string> monList = new List<string>();
    private List<GameObject> spawnList = new List<GameObject>();


    public SkeletonSummonController() : base(Define.SkillType.Count)
    {

    }
    public void SetInfo(int Unitlv, int summonCount ,Unit owner, SkillData data, Vector3 startPos)
    {
        //if (Managers.Data.magicSkillDict.TryGetValue(Unitlv, out SkillData data) == false)
        //{
        //    Debug.LogError("ProjecteController SetInfo Failed");
        //    return;
        //}
        summonidx = summonCount;
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

        for(int ii = 0;  ii < Managers.Game.MonsterTypeList.Count - 1;ii++)
        {
            monList.Add(System.Enum.GetName(typeof(Define.MonsterType), Managers.Game.MonsterTypeList[ii]));
            spawnList.Add(Managers.Resource.Load<GameObject>($"Prefabs/Monster/{monList[ii]}"));

        }

        Debug.Log(spawnList.Count);
        //GameObject go = Managers.Resource.Load<GameObject>("Prefabs/Monster/NormalSkele");
        for(int ii = 0; ii < summonidx; ii++)
        {
            Debug.Log("����");
            yield return wfs;
            int randidx = Random.Range(0, 2);
            Managers.Resource.Instantiate(spawnList[randidx], this.transform.position);
        }

        if (anim != null)
        {
            anim.SetTrigger("Destroy");
        }

    }

}
