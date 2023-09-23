using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSummonController :SkillBase
{
    Unit owener;
    Unit enemy;
    Vector3 pos;
    Vector3 curPos;
    float _speed = 20.0f;
    float _lifeTime = 3.0f;
    Animator anim;

    public SwordSummonController() : base(Define.SkillType.Count)
    {

    }
    public void SetInfo(Unit owner, Unit enemy, SkillData data, Vector3 startPos)
    {
        //if (Managers.Data.magicSkillDict.TryGetValue(Unitlv, out SkillData data) == false)
        //{
        //    Debug.LogError("ProjecteController SetInfo Failed");
        //    return;
        //}

        owener = owner;
        this.enemy = enemy;
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



        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            Destroy(this.gameObject,0.2f);
        }
    }


    public void OnAttack()
    {
        if (enemy != null && enemy.gameObject.tag.Contains("Monster"))
        {
            enemy.TryGetComponent(out Unit mon);
            mon.OnDamage(100);
            Managers.Resource.ResourceEffect(enemy.transform.position, "HitEff");

        }
    }


}
