using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class AttackAfterImageSkill : ActiveSkill
{
    public override void UseSkill(Unit unit)
    {

        if (unit == null )
            return;

        Owner = unit;
        GenerateAttackAfterImage(Managers.Game.MonsterTypeIdDict[MonsterType.EliteCavalry], unit);  // �����ܻ� ����


    }

    public override void UseSkill(Unit unit, Tower tower)
    {
        if (unit == null || tower == null)
            return;


        //��ų�� Ÿ���� �����Ҽ����� ���� �߹� �������θ� 

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
        Debug.Log(id);
        if (Managers.Data.eliteCavalrySkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SkillData = data;
    }

    void GenerateAttackAfterImage(int id, Unit owner)
    {
        if (Managers.Data.eliteCavalrySkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }


        AttackAfterImageController ac = Managers.Resource.Instantiate(data.skillPrefab,Owner.transform).GetComponent<AttackAfterImageController>();
        ac.SetInfo(owner, data);
    }
}
