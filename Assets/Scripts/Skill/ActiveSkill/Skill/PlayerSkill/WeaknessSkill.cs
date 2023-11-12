using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessSkill : ActiveSkill
{
    public override void SkillDataSetting(int id)
    {
        if (Managers.Data.weaknessSkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
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
                enemys[ii].UnitDebuff(SkillData.skillValue, 10.0f, monCtrl.Debuff.DebuffOnOff);
                monCtrl.Debuff.DebuffOnOff(true);

            }
            else if (enemys[ii] is EliteMonsterController eliteMonCtrl)
            {
                enemys[ii].UnitDebuff(SkillData.skillValue, 10.0f, eliteMonCtrl.Debuff.DebuffOnOff);
                eliteMonCtrl.Debuff.DebuffOnOff(true);

            }
        }




        //ÀÌÆåÆ® Ãß°¡
    }

}
