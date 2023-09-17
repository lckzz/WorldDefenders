using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteroSkill : ActiveSkill
{


    public override void UseSkill(Unit unit, List<Unit> enemys)
    {
        if (unit == null || enemys == null)
            return;

        for(int ii = 0; ii < enemys.Count; ii++)
        {
            //����Ʈ�ȿ� �ִ� ����ŭ ���׿��� ��ȯ
            Vector3 pos = unit.transform.position;
            pos.y += 2.0f;

            GenerateMeteor(GlobalData.g_UnitMagicianLv, unit, enemys[ii], pos);
        }

    }

    public override void UseSkill(Unit unit, Tower tower)
    {
        if (unit == null || tower == null)
            return;

        //for (int ii = 0; ii < enemys.Count; ii++)
        //{
        //    //����Ʈ�ȿ� �ִ� ����ŭ ���׿��� ��ȯ
        //    Vector3 pos = unit.transform.position;
        //    pos.y += 2.0f;

        //    GenerateMeteor(GlobalData.g_UnitMagicianLv, unit, enemys[ii], pos);
        //}

    }

    public override void SkillDataSetting(int id)
    {
        if (Managers.Data.magicSkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SkillData = data;
    }

    void GenerateMeteor(int unitLv, Unit owner, Unit enemy, Vector3 spawnPos)
    {
        ////���׿� 3�� ��ȯ
        if (Managers.Data.magicSkillDict.TryGetValue(unitLv, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }
        MeteorController mc = Managers.Resource.Instantiate(data.skillPrefab).GetComponent<MeteorController>();
        mc.SetInfo(unitLv, owner, enemy, data, spawnPos);
    }
}
