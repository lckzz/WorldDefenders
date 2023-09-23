using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPowerSkill : ActiveSkill
{
    public override void UseSkill(Unit unit, List<Unit> enemys)
    {
        if (unit == null || enemys == null)
            return;


        GenerateDark(GlobalData.g_EliteShamanID, unit, enemys);
        

    }

    public override void UseSkill(Unit unit, Tower tower)
    {
        if (unit == null || tower == null)
            return;

        //for (int ii = 0; ii < enemys.Count; ii++)
        //{
        //    //리스트안에 있는 수만큼 메테오를 소환
        //    Vector3 pos = unit.transform.position;
        //    pos.y += 2.0f;

        //    GenerateMeteor(GlobalData.g_UnitMagicianLv, unit, enemys[ii], pos);
        //}

    }

    public override void SkillDataSetting(int id)
    {
        if (Managers.Data.eliteShamanSkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SkillData = data;
    }

    void GenerateDark(int unitLv, Unit owner, List<Unit> enemy)
    {
        ////메테오 3개 소환
        if (Managers.Data.eliteShamanSkillDict.TryGetValue(unitLv, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }
        DarkPowerController dc = Managers.Resource.Instantiate(data.skillPrefab).GetComponent<DarkPowerController>();
        dc.SetInfo(owner, enemy, data);
    }
}
