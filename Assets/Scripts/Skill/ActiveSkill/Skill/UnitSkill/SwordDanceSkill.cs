using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SwordDanceSkill : ActiveSkill
{
    public override void UseSkill(Unit unit, List<Unit> enemys)
    {
        if (unit == null || enemys == null)
            return;

        Debug.Log("持失さしさししさしささ");

        Vector3 pos = unit.transform.position;
        pos.x -= 1.0f;
        Generate(Managers.Game.MonsterTypeIdDict[MonsterType.EliteWarrior], unit, enemys,pos);
        

    }

    public override void UseSkill(Unit unit, Tower tower)
    {
        //throw new System.NotImplementedException();
    }

    public override void SkillDataSetting(int id)
    {
        if (Managers.Data.eliteWarriorSkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SkillData = data;
    }

    void Generate(int id, Unit owner, List<Unit> enemy,Vector3 spawnPos)
    {
        
        ///伊奄 廃鯵
        if (Managers.Data.eliteWarriorSkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SwordDanceControllter sk = Managers.Resource.Instantiate(data.skillPrefab).GetComponent<SwordDanceControllter>();
        sk.SetInfo(id, owner, enemy, data, spawnPos);
    }
}
