using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSummonController : SkillBase
{
    //포탈이 생성되고 생성된 포탈에서 스켈레톤을 생성한다.

    
    Unit owener;
    Vector3 pos;
    Vector3 curPos;
    float _lifeTime = 3.0f;
    Animator anim;
    int summonidx;
    bool firstSummon = false;

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
        StartDestroy(_lifeTime);

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
        GameObject go = Managers.Resource.Load<GameObject>("Prefabs/Monster/NormalSkele");
        for(int ii = 0; ii < summonidx; ii++)
        {
            Debug.Log("테테");
            yield return wfs;
            Managers.Resource.Instantiate(go);
        }


        yield return wfs;

        if (anim != null)
            anim.SetTrigger("Destroy");

    }

}
