using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessSkill : ActiveSkill
{
    PlayerSkillData playerSkillData = null;
    public override void SkillDataSetting(int id)
    {



        if (Managers.Data.weaknessSkillDict.TryGetValue(id, out PlayerSkillData data) == false)
        {
            Debug.LogError("Failed");
            return;
        }

        SkillData = data;
        playerSkillData = SkillData as PlayerSkillData;
        


    }


    public override void UseSkill(List<MonsterBase> enemys)
    {
        for(int ii = 0; ii < enemys.Count; ii++)
        {

            if (enemys[ii] is MonsterController monCtrl)
            {

                if (monCtrl.Debuff is WeaknessDebuff weaknessDebuff)
                {
                    if (monCtrl.IsDie)
                        continue;

                    weaknessDebuff.WeaknessSkillInfo(monCtrl.MoveSpeed, monCtrl.Att);
                    weaknessDebuff.UnitDebuff(playerSkillData.skillValue, playerSkillData.skillDuration);
                    weaknessDebuff.DebuffOnOff(true);
                }


            }
            else if (enemys[ii] is EliteMonsterController eliteMonCtrl)
            {
                if (eliteMonCtrl.Debuff is WeaknessDebuff weaknessDebuff)
                {

                    if (eliteMonCtrl.IsDie)
                        continue;
                    weaknessDebuff.WeaknessSkillInfo(eliteMonCtrl.MoveSpeed, eliteMonCtrl.Att);
                    weaknessDebuff.UnitDebuff(SkillData.skillValue, playerSkillData.skillDuration);
                    weaknessDebuff.DebuffOnOff(true);
                }


            }
        }




        //ÀÌÆåÆ® Ãß°¡
    }

}
