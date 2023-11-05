using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSummonSkill : ActiveSkill
{
    public override void UseSkill(Unit unit, List<Unit> enemys)
    {
        if (unit == null || enemys == null)
            return;

        for (int ii = 0; ii < enemys.Count; ii++)
        {
            //리스트안에 있는 수만큼 메테오를 소환
            Vector3 pos = enemys[ii].transform.position;
            pos.y += 1.5f;

            GenerateSword(1, unit, enemys[ii], pos);
        }
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
        if (Managers.Data.cavalrySkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SkillData = data;
    }

    void GenerateSword(int unitLv, Unit owner, Unit enemy, Vector3 spawnPos)
    {
        ////칼날 소환
        if (Managers.Data.cavalrySkillDict.TryGetValue(unitLv, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }
        
        SwordSummonController ssc = Managers.Resource.Instantiate(data.skillPrefab).GetComponent<SwordSummonController>();
        ssc.SetInfo(owner, enemy, data, spawnPos);
    }
}
