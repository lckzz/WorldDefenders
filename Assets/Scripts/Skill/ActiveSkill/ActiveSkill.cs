using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkill : SkillBase
{
    public float CoolTime { get; set; }
    public bool CoolTimeCheck { get; set; }

    public override bool Init()
    {
        base.Init();
        return true;
    }

    public abstract void SkillDataSetting(int id);



    public virtual void UseSkill(List<MonsterBase> enemys) { }
    public virtual void UseSkill(Unit unit, List<Unit> enemys) { }     //��Ƽ�� ��ų ���
    public virtual void UseSkill(Unit unit, Tower tower) { }
    public virtual void UseSkill(Tower tower) { }
    public virtual void UseSkill(PlayerController player) { }

    public virtual void UseSkill(Unit unit) { }     //��Ƽ�� ��ų ���

}
