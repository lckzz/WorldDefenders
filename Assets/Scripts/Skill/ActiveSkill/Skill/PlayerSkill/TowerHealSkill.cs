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
        //타워의 hp에 접근해서 hp가 0보다 작지않으면 hp를 올려준다.

        Debug.Log("여기엥메넹ㄴ멘메 힐스킬 써짐" + playertower);

        if (playertower == null)
            return;

        PlayerTower tower = playertower as PlayerTower;
        float hp = tower.Hp;

       


        if (playerSkillData != null)
        {


            if (hp > 0)
                hp += tower.MaxHp * (playerSkillData.skillValue / 100.0f);       //타워의 최대체력의 스킬퍼센트만큼 회복해준다.

            if(hp >= tower.MaxHp)
                hp = tower.MaxHp;


            tower.Hp = hp;
            tower.NotifyToHpObserver();
            


        }


        //이펙트 추가
    }




}
