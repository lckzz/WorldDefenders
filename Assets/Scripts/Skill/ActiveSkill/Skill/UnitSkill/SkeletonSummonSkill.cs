using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class SkeletonSummonSkill : ActiveSkill
{
    public override void UseSkill(Unit unit)
    {
        if (unit == null)
            return;


        int summonidx = 2;
        unit.TryGetComponent(out EliteCavalryController ecCtrl);
        List<GameObject> goList = new List<GameObject>();
        goList.Add(ecCtrl.MiniPortal1);
        goList.Add(ecCtrl.MiniPortal2);



        for (int ii = 0; ii < goList.Count; ii++)
        {
            
            Vector3 pos = goList[ii].transform.position;

            GenerateMiniPortal(GlobalData.g_EliteCavalryID, summonidx, unit, pos);
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
        if (Managers.Data.eliteCavalrySkillDict.TryGetValue(id, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SkillData = data;
    }

    void GenerateMiniPortal(int unitLv, int summonCount,Unit owner, Vector3 spawnPos)
    {
        ////포탈 소환
        if (Managers.Data.eliteCavalrySkillDict.TryGetValue(unitLv, out SkillData data) == false)
        {
            Debug.LogError("ProjecteController SetInfo Failed");
            return;
        }

        SkeletonSummonController ssc = Managers.Resource.Instantiate(data.skillPrefab).GetComponent<SkeletonSummonController>();
        ssc.SetInfo(unitLv,summonCount, owner, data, spawnPos);
    }
}
