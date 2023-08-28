using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkill : SkillBase
{
    public float CoolTime { get; set; }
    public bool CoolTimeCheck { get; set; }


    public ActiveSkill() : base(Define.SkillType.Active)
    {
    }

    public override bool Init()
    {
        base.Init();
        return true;
    }




    public abstract void UseSkill(Unit unit, List<Unit> enemys);     //액티브 스킬 사용

  

}
