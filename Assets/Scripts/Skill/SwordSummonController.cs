using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class SwordSummonController :SkillBase
{

    Unit enemy;
    Vector3 pos;
    Vector3 curPos;
    float _lifeTime = 3.0f;
    Animator anim;

    public void SetInfo(Unit owner, Unit enemy, SkillData data, Vector3 startPos)
    {
        //if (Managers.Data.magicSkillDict.TryGetValue(Unitlv, out SkillData data) == false)
        //{
        //    Debug.LogError("ProjecteController SetInfo Failed");
        //    return;
        //}

        Owner = owner;
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
        Debug.Log("테스틈ㄴㅇㅁㄴㅇ");
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
            if (Owner.CriticalCheck())
                mon.OnDamage(Mathf.RoundToInt(Owner.Att * (SkillData.skillValue * 0.01f)) * 2, 0, true);      //넉백은 없고 크리티컬은 터짐
            else
                mon.OnDamage(Mathf.RoundToInt(Owner.Att * (SkillData.skillValue * 0.01f)));      //노크리티컬
            Managers.Resource.ResourceEffect(enemy.transform.position, "HitEff");

        }
    }


}
