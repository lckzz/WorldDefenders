using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealSkill : ActiveSkill
{
    PlayerSkillData playerSkillData = null;



    public override void SkillDataSetting(int id)
    {
        if (Managers.Data.healSkillDict.TryGetValue(id, out PlayerSkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SkillData = data;
        playerSkillData = SkillData as PlayerSkillData;
    }

    public override void UseSkill(Tower playertower)
    {
        //Ÿ���� hp�� �����ؼ� hp�� 0���� ���������� hp�� �÷��ش�.

        Debug.Log("���⿨�޳ߤ���� ����ų ����" + playertower);

        if (playertower == null)
            return;

        PlayerTower tower = playertower as PlayerTower;
        float hp = tower.Hp;

       


        if (playerSkillData != null)
        {


            if (hp > 0)
                hp += tower.MaxHp * (playerSkillData.skillValue / 100.0f);       //Ÿ���� �ִ�ü���� ��ų�ۼ�Ʈ��ŭ ȸ�����ش�.

            if(hp >= tower.MaxHp)
                hp = tower.MaxHp;


            tower.Hp = hp;
            tower.NotifyToHpObserver();
            


        }


        //����Ʈ �߰�
    }




}
