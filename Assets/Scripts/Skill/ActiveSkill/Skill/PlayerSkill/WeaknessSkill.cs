using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessSkill : ActiveSkill
{
    public override void SkillDataSetting(int id)
    {
        if (Managers.Data.weaknessSkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("Failed");
            return;
        }

        SkillData = data;
    }


    public override void UseSkill(List<Unit> enemys)
    {
        for(int ii = 0; ii < enemys.Count; ii++)
        {
            if (enemys[ii] is MonsterController monCtrl)
            {
                if(monCtrl.Debuff is WeaknessDebuff weaknessDebuff)
                {
                    Debug.Log(monCtrl.name);
                    if (monCtrl.IsDie)
                        return;

                    weaknessDebuff.WeaknessSkillInfo(monCtrl.MoveSpeed, monCtrl.Att);
                    weaknessDebuff.UnitDebuff(SkillData.skillValue, 10.0f);
                    weaknessDebuff.DebuffOnOff(true);
                }


            }
            else if (enemys[ii] is EliteMonsterController eliteMonCtrl)
            {
                if (eliteMonCtrl.Debuff is WeaknessDebuff weaknessDebuff)
                {
                    if (eliteMonCtrl.IsDie)
                        return;
                    weaknessDebuff.WeaknessSkillInfo(eliteMonCtrl.MoveSpeed, eliteMonCtrl.Att);
                    weaknessDebuff.UnitDebuff(SkillData.skillValue, 10.0f);
                    weaknessDebuff.DebuffOnOff(true);
                }


            }
        }




        //ÀÌÆåÆ® Ãß°¡
    }

}
