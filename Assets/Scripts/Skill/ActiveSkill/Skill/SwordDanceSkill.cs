using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDanceSkill : ActiveSkill
{
    public override void UseSkill(Unit unit, List<Unit> enemys)
    {
        if (unit == null || enemys == null)
            return;

        Vector3 pos = unit.transform.position;
        pos.x -= 1.0f;
        Generate(GlobalData.g_EliteWarriorID, unit, enemys,pos);
        

    }


    void Generate(int id, Unit owner, List<Unit> enemy,Vector3 spawnPos)
    {
        ///검기 한개
        if (Managers.Data.eliteWarriorSkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }
        SwordDanceControllter sk = Managers.Resource.Instantiate(data.skillPrefab).GetComponent<SwordDanceControllter>();
        sk.SetInfo(id, owner, enemy, data, spawnPos);
    }
}
