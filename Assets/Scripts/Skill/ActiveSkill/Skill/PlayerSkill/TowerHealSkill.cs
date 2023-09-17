using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealSkill : ActiveSkill
{
    public override void SkillDataSetting(int id)
    {
        if (Managers.Data.healSkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SkillData = data;
    }

    public override void UseSkill(Tower playertower)
    {
        //Ÿ���� hp�� �����ؼ� hp�� 0���� ���������� hp�� �÷��ش�.
        if (playertower == null)
            return;

        PlayerTower tower = playertower as PlayerTower;
        float hp = tower.GetSetHp;
        if(SkillData != null)
        {
            if (hp > 0)
                hp += SkillData.skillValue;

            if(hp >= tower.GetMaxHp)
                hp = tower.GetMaxHp;
            tower.GetSetHp = hp;


        }


        //����Ʈ �߰�
    }




}
