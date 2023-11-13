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
        float hp = tower.Hp;
        if(SkillData != null)
        {
            if (hp > 0)
                hp += tower.MaxHp * (SkillData.skillValue / 100);       //Ÿ���� �ִ�ü���� ��ų�ۼ�Ʈ��ŭ ȸ�����ش�.

            if(hp >= tower.MaxHp)
                hp = tower.MaxHp;
            tower.Hp = hp;


        }


        //����Ʈ �߰�
    }




}
